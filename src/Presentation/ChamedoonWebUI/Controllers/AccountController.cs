using Chamedoon.Application.Services.Account.Login.Command;
using Chamedoon.Application.Services.Account.Login.Query;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Users.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Configs;
using ChamedoonWebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NuGet.Common;
using System;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ChamedoonWebUI.Controllers;

[Route("auth")]
public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly UrlsConfig _urls;
    private readonly IEmailService _emailService;
    private readonly IMemoryCache _memoryCache;

    private const string LoginCodePurpose = "login";
    private const string RegisterCodePurpose = "register";
    private static readonly TimeSpan VerificationCodeExpiration = TimeSpan.FromMinutes(5);

    public AccountController(IMediator mediator, IOptions<UrlsConfig> urlOption, IEmailService emailService, IMemoryCache memoryCache)
    {
        _mediator = mediator;
        _urls = urlOption.Value;
        _emailService = emailService;
        _memoryCache = memoryCache;
    }

    private string GenerateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(1000, 10000).ToString("D4");
    }

    private async Task<bool> SendVerificationCodeAsync(string email, string purpose)
    {
        var code = GenerateVerificationCode();
        var cacheKey = $"{purpose}:{email.ToLowerInvariant()}";
        _memoryCache.Set(cacheKey, code, VerificationCodeExpiration);

        var subject = "کد تایید چمدون";
        var body = $"<p>کد تایید شما: <strong>{code}</strong></p><p>این کد به مدت ۵ دقیقه معتبر است.</p>";
        await _emailService.SendMail(email, subject, body);
        return true;
    }

    private bool ValidateVerificationCode(string email, string purpose, string code)
    {
        var cacheKey = $"{purpose}:{email.ToLowerInvariant()}";
        if (_memoryCache.TryGetValue<string>(cacheKey, out var storedCode) && storedCode == code)
        {
            _memoryCache.Remove(cacheKey);
            return true;
        }

        return false;
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

        #region Login
    [Route("login")]
    public IActionResult Login()
    {
        ViewData["LoginNonce"] = PrepareRequestNonce(nameof(Login));
        return View();
    }
    [Route("login")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserViewModel loginModel, string requestNonce)
    {
        if (!IsRequestNonceValid(nameof(Login), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکرار یا نامعتبر.");
            ViewData["LoginNonce"] = PrepareRequestNonce(nameof(Login));
            return View(loginModel);
        }

        if (!ModelState.IsValid)
        {
            ViewData["LoginNonce"] = PrepareRequestNonce(nameof(Login));
            return View(loginModel);
        }

        var loginResult = await _mediator.Send(new ManageLoginUserCommand { LoginUser = loginModel });
        if (!loginResult.IsSuccess || loginResult.Result is null)
        {
            ModelState.AddModelError(string.Empty, loginResult.Message ?? "خطا در ورود");
            ViewData["LoginNonce"] = PrepareRequestNonce(nameof(Login));
            return View(loginModel);
        }

        var signInResult = await _mediator.Send(new SignInUserCommand { UserId = loginResult.Result.Id.ToString(), IsPersistent = loginModel.RememberMe });
        if (!signInResult.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, signInResult.Message);
            ViewData["LoginNonce"] = PrepareRequestNonce(nameof(Login));
            return View(loginModel);
        }

        return RedirectToAction("Index", "Home");
    }

    [Route("login/code")]
    public IActionResult LoginWithCode()
    {
        ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
        return View(new LoginWithCodeViewModel());
    }

    [Route("login/code")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWithCode(LoginWithCodeViewModel loginModel, string requestNonce, string? actionType)
    {
        if (!IsRequestNonceValid(nameof(LoginWithCode), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکرار یا نامعتبر.");
            ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
            return View(loginModel);
        }

        if (string.Equals(actionType, "sendCode", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.Remove(nameof(LoginWithCodeViewModel.VerificationCode));
            ModelState.Remove(nameof(LoginWithCodeViewModel.CodeSent));

            if (!ModelState.IsValid)
            {
                ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
                return View(loginModel);
            }

            var existingUser = await _mediator.Send(new GetUserQuery { Email = loginModel.Email });
            if (existingUser.IsSuccess is false || existingUser.Result is null)
            {
                ModelState.AddModelError(string.Empty, "کاربری با این ایمیل یافت نشد.");
                ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
                return View(loginModel);
            }

            await SendVerificationCodeAsync(loginModel.Email, LoginCodePurpose);
            ViewBag.InfoMessage = "کد ورود ارسال شد.";
            loginModel.CodeSent = true;
            ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
            return View(loginModel);
        }

        if (!ModelState.IsValid)
        {
            loginModel.CodeSent = loginModel.CodeSent || !string.IsNullOrWhiteSpace(loginModel.VerificationCode);
            ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
            return View(loginModel);
        }

        if (!ValidateVerificationCode(loginModel.Email, LoginCodePurpose, loginModel.VerificationCode ?? string.Empty))
        {
            ModelState.AddModelError(nameof(LoginWithCodeViewModel.VerificationCode), "کد وارد شده نامعتبر است یا منقضی شده است.");
            loginModel.CodeSent = true;
            ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
            return View(loginModel);
        }

        var userLookup = await _mediator.Send(new GetUserQuery { Email = loginModel.Email });
        if (userLookup.IsSuccess is false || userLookup.Result is null)
        {
            ModelState.AddModelError(string.Empty, "کاربری با این ایمیل یافت نشد.");
            loginModel.CodeSent = true;
            ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
            return View(loginModel);
        }

        var signInResult = await _mediator.Send(new SignInUserCommand { UserId = userLookup.Result.Id.ToString(), IsPersistent = loginModel.RememberMe });
        if (!signInResult.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, signInResult.Message);
            loginModel.CodeSent = true;
            ViewData["LoginWithCodeNonce"] = PrepareRequestNonce(nameof(LoginWithCode));
            return View(loginModel);
        }

        return RedirectToAction("Index", "Home");
    }
    #endregion


        #region Register

    [Route("register")]
    public IActionResult Register()
    {
        ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
        return View(new RegisterUser_VM { Stage = RegisterStep.EnterEmail });
    }

    [Route("register")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUser_VM register, string requestNonce, string? actionType)
    {
        if (!IsRequestNonceValid(nameof(Register), requestNonce))
        {
            ModelState.AddModelError(string.Empty, "درخواست تکراری یا نامعتبر.");
            ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
            return View(register);
        }

        if (string.Equals(actionType, "sendCode", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.Remove(nameof(RegisterUser_VM.VerificationCode));
            ModelState.Remove(nameof(RegisterUser_VM.UserName));
            ModelState.Remove(nameof(RegisterUser_VM.Password));
            ModelState.Remove(nameof(RegisterUser_VM.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                register.Stage = RegisterStep.EnterEmail;
                ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
                return View(register);
            }

            var duplicationCheck = await _mediator.Send(new CheckDuplicatedEmailQuery { Email = register.Email });
            if (!duplicationCheck.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, duplicationCheck.Message);
                register.Stage = RegisterStep.EnterEmail;
                ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
                return View(register);
            }

            await SendVerificationCodeAsync(register.Email, RegisterCodePurpose);
            ViewBag.RegisterCodeSent = true;
            ViewBag.InfoMessage = "کد تایید برای ایجاد حساب ارسال شد.";
            register.Stage = RegisterStep.VerifyCode;
            ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
            return View(register);
        }

        if (string.Equals(actionType, "verifyCode", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.Remove(nameof(RegisterUser_VM.UserName));
            ModelState.Remove(nameof(RegisterUser_VM.Password));
            ModelState.Remove(nameof(RegisterUser_VM.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                register.Stage = RegisterStep.VerifyCode;
                ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
                return View(register);
            }

            if (!ValidateVerificationCode(register.Email, RegisterCodePurpose, register.VerificationCode ?? string.Empty))
            {
                ModelState.AddModelError(nameof(RegisterUser_VM.VerificationCode), "کد وارد شده نامعتبر است یا منقضی شده است.");
                register.Stage = RegisterStep.VerifyCode;
                ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
                return View(register);
            }

            TempData[nameof(RegisterStep)] = RegisterStep.SetCredentials.ToString();
            TempData["VerifiedEmail"] = register.Email;
            register.Stage = RegisterStep.SetCredentials;
            register.VerificationCode = string.Empty;
            ViewBag.InfoMessage = "کد تایید شد. نام کاربری و رمز عبور خود را تعیین کنید.";
            ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
            return View(register);
        }

        var verifiedEmail = TempData.Peek("VerifiedEmail") as string;
        var verifiedStageValue = TempData.Peek(nameof(RegisterStep)) as string;
        RegisterStep? verifiedStage = Enum.TryParse<RegisterStep>(verifiedStageValue, out var stage) ? stage : null;

        ModelState.Remove(nameof(RegisterUser_VM.VerificationCode));

        if (!ModelState.IsValid || verifiedEmail != register.Email || verifiedStage != RegisterStep.SetCredentials)
        {
            if (verifiedEmail != register.Email || verifiedStage != RegisterStep.SetCredentials)
            {
                ModelState.AddModelError(string.Empty, "ابتدا ایمیل خود را تایید کنید.");
                register.Stage = RegisterStep.VerifyCode;
            }
            else
            {
                register.Stage = RegisterStep.SetCredentials;
            }

            ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
            return View(register);
        }

        TempData.Remove("VerifiedEmail");
        TempData.Remove(nameof(RegisterStep));

        var response = await _mediator.Send(new ManageRegisterUserCommand { RegisterUser = register });

        if (!response.IsSuccess && response.Message != null)
        {
            ViewData["ErrorMessage"] = string.Join(", ", response.Message);
            register.Stage = RegisterStep.SetCredentials;
            TempData[nameof(RegisterStep)] = RegisterStep.SetCredentials.ToString();
            TempData["VerifiedEmail"] = register.Email;
            ViewData["RegisterNonce"] = PrepareRequestNonce(nameof(Register));
            return View(register);
        }

        var user = await _mediator.Send(new GetUserQuery { Email = register.Email });
        if (user.IsSuccess && user.Result != null)
        {
            await _mediator.Send(new SignInUserCommand { UserId = user.Result.Id.ToString(), IsPersistent = true });
        }

        return RedirectToAction("Index", "Home");
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
    [Route("emailverify")]
    public IActionResult EmailVerification()
    {
        return View();
    }

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
    #endregion
}