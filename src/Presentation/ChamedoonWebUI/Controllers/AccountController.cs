﻿using Chamedoon.Application.Services.Account.Login.Command;
using Chamedoon.Application.Services.Account.Login.Query;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Users.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Email.Query;
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
using System.Security.Claims;

namespace ChamedoonWebUI.Controllers;

[Route("auth")]
public class AccountController : Controller
{
    private readonly IMediator mediator;

    public AccountController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region Login
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }
    [Route("login")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserViewModel register)
    {
        if (ModelState.IsValid)
        {
            var user = (await mediator.Send(new ManageLoginUserCommand { LoginUser = register }));
            if (user.IsSuccess is false || user.Result is null)
            {
                ModelState.AddModelError(string.Empty, user.Message);
                return View(register);
            }
            return RedirectToAction("Index", "Home");
        }
        return View(register);
    }
    #endregion

    #region Register

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("register")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUser_VM register)
    {
        if (ModelState.IsValid)
        {
            var response = await mediator.Send(new ManageRegisterUserCommand { RegisterUser = register });

            if (!response.IsSuccess && response.Message != null)
            {
                ViewData["ErrorMessage"] = string.Join(", ", response.Message);
                return View(register);
            }

            return RedirectToAction("EmailVerification");
        }
        return View(register);
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
        return View();
    }

    [Route("ChangePass")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        model.UserId = userId;
        var result = await mediator.Send(new ChangePasswordCommand { ChangePasswordViewModel = model });

        if (result.IsSuccess)
            return RedirectToAction("login", "Account");

        ModelState.AddModelError("", result.Message);

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
        var result = await mediator.Send(new ConfirmEmailQuery { Token = token, UserId = userId });
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        var user = await mediator.Send(new GetUserQuery { Id = long.Parse(userId) });

        if (user.IsSuccess && user.Result != null)
        {
            var signInResult = await mediator.Send(new SignInUserCommand { UserId = userId, IsPersistent = true });
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
        var redirectUrl = Url.Action("google-callback", "auth");

        var result = await mediator.Send(new ExternalLoginCommand { Provider = GoogleDefaults.AuthenticationScheme, RedirectUrl = redirectUrl });
        if (!result.IsSuccess || result.Result is null)
            return RedirectToAction("register", "Account");

        return Challenge(result.Result, GoogleDefaults.AuthenticationScheme);
    }
    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
    {
        var result = mediator.Send(new ExternalLoginCallbackQuery()).Result;

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
        return View();
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(new ForgotPasswordQuery { Email = model.Email });

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "ایمیلی با این آدرس یافت نشد.");
            return View(model);
        }

        ViewBag.Message = "لینک بازیابی رمز عبور به ایمیل شما ارسال شد.";
        return View();
    }
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await mediator.Send(new ResetPasswordCommand
        {
            Email = model.Email,
            Token = model.Token,
            NewPassword = model.NewPassword
        });

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "خطا در تغییر رمز عبور. لطفاً دوباره امتحان کنید.");
            return View(model);
        }

        return RedirectToAction("Login");
    }
    #endregion
}