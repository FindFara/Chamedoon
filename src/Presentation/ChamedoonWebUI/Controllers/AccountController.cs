using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Login.Command;

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
                var user = (await mediator.Send(new ManageLoginUserQuery { LoginUser = register })).Result;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email)
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
              var response = await mediator
                    .Send(new ManageRegisterUserCommand { RegisterUser = register });
            }
            else
            {
                return View(register);
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Logout
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
