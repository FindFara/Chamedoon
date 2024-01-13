using Chamedoon.Application.Services.Account.Command;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Chamedoon.WebAPI.Controllers;
public class AccountController : Controller
{
    public IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Login

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("login")]
    public async Task<IActionResult> Login([FromForm] LoginUser_VM model, string retutnURL = null)
    {
        var checkMatch = await _mediator.Send(new CheckUserNameAndPasswordMatchQuery { LoginUser_VM = model });
        if (checkMatch.Code != 0)
            ModelState.AddModelError(model.Password, checkMatch.Message);

        ViewData["retutnURL"] = retutnURL;
        if (ModelState.IsValid)
        {
            var user = await _mediator.Send(new GetUserByUserNameQuery { UserName = model.UserName });
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Result.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Result.UserName),
                new Claim(ClaimTypes.Email,user.Result.Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };
            await HttpContext.SignInAsync(principal, properties);

            if (!string.IsNullOrEmpty(retutnURL) && Url.IsLocalUrl(retutnURL))
                return Ok(new { returnUrl = retutnURL });

            return Ok(new { returnUrl = "Index/Home" });
        }
        return BadRequest(ModelState);
    }

    #endregion Login

    #region Register

    [Route("register")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegisterUser_VM register)
    {
        if (ModelState.IsValid)
        {
            // Check Duplicated Email
            var checkEmail = await _mediator.Send(new CheckDuplicatedEmailQuery { Email = register.Email });
            if (checkEmail.Result == false)
            {
                ModelState.AddModelError(nameof(register.Email), checkEmail.Message);
                return BadRequest(ModelState);
            }

            // Check Duplicated UserName
            var checkUserName = await _mediator.Send(new CheckDuplicatedUserNameQuery { UserName = register.UserName });
            if (checkUserName.Result == false)
            {
                ModelState.AddModelError(nameof(register.UserName), checkUserName.Message);
                return BadRequest(ModelState);
            }

            var regisrer = await _mediator.Send(new RegisterUserCommand { RegisterUser_VM = register });
            if (regisrer.Result == false)
            {
                ModelState.AddModelError(nameof(register.Password), regisrer.Message);
                return BadRequest(ModelState);
            }
        }
        else
        {
            return BadRequest(ModelState);
        }

        //TODO: Activation Send Email
        return Ok(new { message = "ثبت نام با موفقیت انجام شد !" });
    }

    #endregion Register

    #region Logout

    [ValidateAntiForgeryToken]
    [Route("Logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await HttpContext.SignOutAsync();
            return Ok(new { message = "خروج با موفقیت انجام شد" });
        }
        catch (Exception ex)
        {
            //TODO: Log the exception using a logging framework like Serilog or NLog
            return StatusCode(500, "خطا در فرایند خروج");
        }
    }
    #endregion

    [Route("AccessDenied")]
    public IActionResult AccessDenied()
    {
        return StatusCode(403, "عدم دسترسی");

    }
}