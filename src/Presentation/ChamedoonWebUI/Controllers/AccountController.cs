using Chamedoon.Application.Services.Account.Login.Command;
using Chamedoon.Application.Services.Account.Login.Query;
using Chamedoon.Application.Services.Account.Users.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Configs;
using ChamedoonWebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace ChamedoonWebUI.Controllers;

[Route("auth")]
public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly UrlsConfig _urls;


    public AccountController(IMediator mediator, IOptions<UrlsConfig> urlOption)
    {
        _mediator = mediator;
        _urls = urlOption.Value;
    }

    private string PrepareRequestNonce(string actionKey)
    {
        var nonce = Guid.NewGuid().ToString("N");
        TempData[actionKey] = nonce;
        return nonce;
    }

    private bool IsRequestNonceValid(string actionKey, string? nonce)
    {
        var storedNonce = TempData.Peek(actionKey) as string;
        if (string.IsNullOrEmpty(storedNonce) || storedNonce != nonce)
        {
            return false;
        }

        TempData.Remove(actionKey);
        return true;
    }

    private string GetSafeReturnUrl(string? returnUrl)
    {
        var fallbackUrl = Url.Action("Index", "Home") ?? "/";

        var candidateUrl = string.IsNullOrWhiteSpace(returnUrl)
            ? Request.Headers["Referer"].ToString()
            : returnUrl;

        if (!string.IsNullOrWhiteSpace(candidateUrl))
        {
            if (Url.IsLocalUrl(candidateUrl) && !candidateUrl.Contains("/auth/register", StringComparison.OrdinalIgnoreCase))
            {
                return candidateUrl;
            }

            if (Uri.TryCreate(candidateUrl, UriKind.Absolute, out var absoluteUri)
                && string.Equals(absoluteUri.Host, Request.Host.Host, StringComparison.OrdinalIgnoreCase))
            {
                var relative = absoluteUri.PathAndQuery;
                if (Url.IsLocalUrl(relative) && !relative.Contains("/auth/register", StringComparison.OrdinalIgnoreCase))
                {
                    return relative;
                }
            }
        }

        return fallbackUrl;
    }

    #region PhoneLogin
    [Route("login")]
    public IActionResult Login(string? returnUrl = null, string? message = null)
    {
        return RedirectToAction(nameof(PhoneLogin), new { returnUrl, message });
    }

    [Route("register")]
    public IActionResult Register(string? returnUrl = null, string? message = null)
    {
        return RedirectToAction(nameof(PhoneLogin), new { returnUrl, message });
    }

    [Route("phone")]
    public IActionResult PhoneLogin(string? returnUrl = null, string? message = null)
    {
        ViewData["PhoneLoginNonce"] = PrepareRequestNonce(nameof(PhoneLogin));
        ViewData["ReturnUrl"] = GetSafeReturnUrl(returnUrl);
        return View("PhoneLogin", new PhoneLoginViewModel { AlertMessage = message });
    }

    [Route("phone/verify")]
    [HttpGet]
    public IActionResult VerifyPhoneCode(string phoneNumber, string? returnUrl = null, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return RedirectToAction(nameof(PhoneLogin), new { returnUrl });
        }

        ViewData["PhoneLoginNonce"] = PrepareRequestNonce(nameof(VerifyPhoneCode));
        ViewData["ReturnUrl"] = GetSafeReturnUrl(returnUrl);
        TempData["PendingPhoneNumber"] = phoneNumber;
        TempData.Keep("PendingPhoneNumber");

        return View("VerifyPhoneCode", new PhoneLoginViewModel
        {
            PhoneNumber = phoneNumber,
            CodeSent = true,
            AlertMessage = message ?? "کد تایید برای شما ارسال شد."
        });
    }

    [Route("phone/send")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendPhoneCode(PhoneLoginViewModel model, string requestNonce, string? returnUrl)
    {
        if (!IsRequestNonceValid(nameof(PhoneLogin), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
        }
        else if (ModelState.IsValid)
        {
            var response = await _mediator.Send(new SendPhoneVerificationCodeCommand { PhoneNumber = model.PhoneNumber });

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                return RedirectToAction(nameof(VerifyPhoneCode), new
                {
                    phoneNumber = model.PhoneNumber,
                    returnUrl,
                    message = "کد تایید برای شما ارسال شد."
                });
            }
        }

        ViewData["PhoneLoginNonce"] = PrepareRequestNonce(nameof(PhoneLogin));
        ViewData["ReturnUrl"] = GetSafeReturnUrl(returnUrl);
        model.Code ??= string.Empty;
        return View("PhoneLogin", model);
    }

    [Route("phone/resend")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendPhoneCode(PhoneLoginViewModel model, string requestNonce, string? returnUrl)
    {
        if (!IsRequestNonceValid(nameof(VerifyPhoneCode), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
        }
        else
        {
            var pendingPhone = (TempData.Peek("PendingPhoneNumber") as string) ?? model.PhoneNumber;

            if (string.IsNullOrWhiteSpace(pendingPhone))
            {
                ModelState.AddModelError(string.Empty, "شماره موبایل یافت نشد.");
                TempData.Remove("PendingPhoneNumber");
            }
            else
            {
                TempData["PendingPhoneNumber"] = pendingPhone;
                TempData.Keep("PendingPhoneNumber");

                model.PhoneNumber = pendingPhone;

            var response = await _mediator.Send(new SendPhoneVerificationCodeCommand
            {
                    PhoneNumber = pendingPhone
            });

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                return RedirectToAction(nameof(VerifyPhoneCode), new
                {
                    phoneNumber = pendingPhone,
                    returnUrl,
                    message = "کد تایید مجدداً ارسال شد."
                });
            }
            }
        }

        ViewData["PhoneLoginNonce"] = PrepareRequestNonce(nameof(VerifyPhoneCode));
        ViewData["ReturnUrl"] = GetSafeReturnUrl(returnUrl);
        model.CodeSent = true;
        model.AlertMessage ??= "کد تایید برای شما ارسال شد.";
        return View("VerifyPhoneCode", model);
    }

    [Route("phone/verify")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyPhoneCode(PhoneLoginViewModel model, string requestNonce, string? returnUrl)
    {
        if (!IsRequestNonceValid(nameof(VerifyPhoneCode), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
        }

        if (string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            var pendingPhone = TempData.Peek("PendingPhoneNumber") as string;
            if (!string.IsNullOrWhiteSpace(pendingPhone))
            {
                model.PhoneNumber = pendingPhone;
            }
        }

        if (string.IsNullOrWhiteSpace(model.Code))
        {
            ModelState.AddModelError(nameof(model.Code), "کد تایید را وارد کنید.");
        }

        if (ModelState.IsValid)
        {
            var response = await _mediator.Send(new VerifyPhoneLoginCommand
            {
                PhoneNumber = model.PhoneNumber,
                Code = model.Code ?? string.Empty
            });

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                var safeReturnUrl = GetSafeReturnUrl(returnUrl);
                return LocalRedirect(safeReturnUrl);
            }
        }

        if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            TempData["PendingPhoneNumber"] = model.PhoneNumber;
            TempData.Keep("PendingPhoneNumber");
        }

        ViewData["PhoneLoginNonce"] = PrepareRequestNonce(nameof(VerifyPhoneCode));
        ViewData["ReturnUrl"] = GetSafeReturnUrl(returnUrl);
        model.CodeSent = true;
        model.AlertMessage ??= "کد تایید برای شما ارسال شد.";
        return View("VerifyPhoneCode", model);
    }
    #endregion

    #region Logout
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region ChangePassword
    [Route("ChangePass")]
    public IActionResult ChangePassword()
    {
        ViewData["ChangePasswordNonce"] = PrepareRequestNonce(nameof(ChangePassword));
        return View();
    }

    [Route("ChangePass")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, string requestNonce)
    {
        if (!IsRequestNonceValid(nameof(ChangePassword), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
            ViewData["ChangePasswordNonce"] = PrepareRequestNonce(nameof(ChangePassword));
            return View(model);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        model.UserId = userId;
        var result = await _mediator.Send(new ChangePasswordCommand { ChangePasswordViewModel = model });

        if (result.IsSuccess)
            return RedirectToAction("login", "Account");

        ModelState.AddModelError("", result.Message);
        ViewData["ChangePasswordNonce"] = PrepareRequestNonce(nameof(ChangePassword));

        return View(model);
    }
    #endregion

    #region ConfirmEmail
    [HttpGet]
    [Route("Confirmemail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (!long.TryParse(userId, out var parsedUserId))
        {
            return BadRequest("Invalid user identifier.");
        }

        var result = await _mediator.Send(new ConfirmEmailQuery { Token = token, UserId = userId });
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        var user = await _mediator.Send(new GetUserQuery { Id = parsedUserId });

        if (user.IsSuccess && user.Result != null)
        {
            var signInResult = await _mediator.Send(new SignInUserCommand { UserId = userId, IsPersistent = true });
            if (!signInResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, signInResult.Message);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        return View();

    }
    #endregion

    #region GoogleAuth
    [HttpGet("login-google")]
    public async Task<IActionResult> LoginWithGoogle()
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "Account");

        var result = await _mediator.Send(new ExternalLoginCommand { Provider = GoogleDefaults.AuthenticationScheme, RedirectUrl = redirectUrl });
        if (!result.IsSuccess || result.Result is null)
            return RedirectToAction("register", "Account");

        return Challenge(result.Result, GoogleDefaults.AuthenticationScheme);
    }
    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
    {
        var result = await _mediator.Send(new ExternalLoginCallbackQuery());

        if (result.IsSuccess)
        {
            return LocalRedirect(returnUrl);
        }

        TempData["ErrorMessage"] = result.Message;
        return RedirectToAction(nameof(Register));
    }
    #endregion

    #region ForgotPassword

    /*
    [HttpGet("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword()
    {
        ViewData["ForgotPasswordNonce"] = PrepareRequestNonce(nameof(ForgotPassword));
        return View(new ForgotPasswordQuery());
    }

    [HttpPost("ForgotPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordQuery model, string requestNonce)
    {
        if (!IsRequestNonceValid(nameof(ForgotPassword), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
            ViewData["ForgotPasswordNonce"] = PrepareRequestNonce(nameof(ForgotPassword));
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            ViewData["ForgotPasswordNonce"] = PrepareRequestNonce(nameof(ForgotPassword));
            return View(model);
        }

        var resetlink = $"{_urls.AppUrl}/auth/ResetPassword";
        model.ResetLinkAction = resetlink;
        var result = await _mediator.Send(model);

        if (result.IsSuccess)
            ViewBag.Message = "ایمیل ارسال شد.";
        else
            ViewBag.Message = "کاربری با این ایمیل یافت نشد.";
        ViewData["ForgotPasswordNonce"] = PrepareRequestNonce(nameof(ForgotPassword));
        return View(model);
    }
    [HttpGet("ResetPassword")]
    public IActionResult ResetPassword(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid password reset request.");
        }
        var model = new ResetPasswordViewModel { Email = email  , Token = token };
        ViewData["ResetPasswordNonce"] = PrepareRequestNonce(nameof(ResetPassword));
        return View(model);
    }
    [HttpPost("ResetPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string requestNonce)
    {
        if (!IsRequestNonceValid(nameof(ResetPassword), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
            ViewData["ResetPasswordNonce"] = PrepareRequestNonce(nameof(ResetPassword));
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            ViewData["ResetPasswordNonce"] = PrepareRequestNonce(nameof(ResetPassword));
            return View(model);
        }

        var result = await _mediator.Send(new ResetPasswordCommand
        {
            Email = model.Email,
            Token = model.Token,
            NewPassword = model.NewPassword
        });

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "خطا در تغییر رمز عبور. لطفاً دوباره امتحان کنید.");
            ViewData["ResetPasswordNonce"] = PrepareRequestNonce(nameof(ResetPassword));
            return View(model);
        }
        return RedirectToAction("Login");
    }
    */
    #endregion
}