using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Login.Command;
using Microsoft.AspNetCore.Identity;

namespace ChamedoonWebUI.Controllers
{
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

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Result.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Result.Email)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = register.RememberMe
                };
                await HttpContext.SignInAsync(principal, properties);

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
                return RedirectToAction("Login", "Account");
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

    }
}
