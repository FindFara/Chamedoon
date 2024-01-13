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


public class AccountController : ApiControllerBase
{
    public IMediator mediator;

    public AccountController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    #region Register

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] ManageRegisterUserCommand request)
    {
        return Ok(await mediator.Send(request));
    }

    #endregion Register

    #region Logout

    //[ValidateAntiForgeryToken]
    //[Route("Logout")]
    //[HttpPost]
    //public async Task<IActionResult> Logout()
    //{
    //    try
    //    {
    //        await HttpContext.SignOutAsync();
    //        return Ok(new { message = "خروج با موفقیت انجام شد" });
    //    }
    //    catch (Exception ex)
    //    {
    //        //TODO: Log the exception using a logging framework like Serilog or NLog
    //        return StatusCode(500, "خطا در فرایند خروج");
    //    }
    //}
    #endregion
    #region Login

    //[AllowAnonymous]
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Login([FromBody] LoginUser_VM model)
    //{
    //    var checkMatch = await _mediator.Send(new CheckUserNameAndPasswordMatchQuery { LoginUser_VM = model });
    //    if (checkMatch.Code != 0)
    //        return BadRequest(model);

    //    var user = await _mediator.Send(new GetUserByUserNameQuery { UserName = model.UserName });
    //        var claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.NameIdentifier,user.Result.Id.ToString()),
    //            new Claim(ClaimTypes.Name,user.Result.UserName),
    //            new Claim(ClaimTypes.Email,user.Result.Email)
    //        };
    //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //        var principal = new ClaimsPrincipal(identity);
    //        var properties = new AuthenticationProperties
    //        {
    //            IsPersistent = model.RememberMe
    //        };
    //        await HttpContext.SignInAsync(principal, properties);

    //        return Ok(new { returnUrl = "Index/Home" });
    //    }
    //    return BadRequest(ModelState);
    //}

    #endregion Login
}