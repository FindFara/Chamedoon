using Chamedoon.Application.Services.Account.Login.Command;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Users.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Email.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Login(LoginUser_VM register)
    {
        if (ModelState.IsValid)
        {
            var user = (await mediator.Send(new ManageLoginUserQuery { LoginUser = register }));
            if (user.IsSuccess is false || user.Result is null)
            {
                ModelState.AddModelError(string.Empty, user.Message);
                return View(register);
            }

            return await SetAuthenticationCookie(user.Result.Id, user.Result.UserName, user.Result.Email, register.RememberMe);

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

            //return await SetAuthenticationCookie(user.Result.Id, user.Result.UserName, user.Result.Email,true);
        }
        return View();

    }
    public async Task<IActionResult> SetAuthenticationCookie(long id, string username, string email, bool rememberMe = false)
    {
        var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Name, username)
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            IsPersistent = rememberMe
        };

        await HttpContext.SignInAsync(principal, properties);

        return RedirectToAction("Index", "Home");
    }
}