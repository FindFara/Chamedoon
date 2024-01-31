using Chamedoon.Application.Services.Account.Authentication;
using Chamedoon.Application.Services.Account.Authentication.Query;
using Chamedoon.Application.Services.Account.Command;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Application.Services.Admin.UserManagement.Query;
using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chamedoon.WebAPI.Controllers;

public class AccountController : ApiControllerBase
{
    public IMediator mediator;

    public AccountController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] ManageRegisterUserCommand request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] ManageLoginUserQuery request)
    {
        //Get user
        var user = await mediator.Send(new GetUserQuery { UserName = request.LoginUser.UserName });
        if (user.Result is null)
            return Unauthorized(user);

        //Get roles
        var userRoles = await mediator.Send(new GetUserRolesQuery { UserName = user.Result.UserName });
         if (userRoles.Result is null)
            return Unauthorized(userRoles);

        var authClaims = new List<Claim>
        {
             new Claim(ClaimTypes.Name, user.Result.UserName ?? ""),
             new Claim(ClaimTypes.Email, user.Result.Email ?? ""),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        foreach (var userRole in userRoles.Result)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        
        //Get token
        var token = (await mediator.Send(new GenerateJsonWebTokenQuery { Claims = authClaims })).Result;
        if (token is null)
            return Unauthorized(token);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });

    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
    {
        return Ok(await mediator.Send(request));
    }
}