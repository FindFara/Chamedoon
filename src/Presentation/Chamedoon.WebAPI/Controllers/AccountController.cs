using Chamedoon.Application.Services.Account.Login.Command;
using Chamedoon.Application.Services.Account.Login.Query;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Email.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chamedoon.WebAPI.Controllers;

[Authorize]
public class AccountController : ApiControllerBase
{
    public IMediator mediator;
    private readonly ILogger logger;

    public AccountController(IMediator mediator , ILogger<AccountController> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] ManageRegisterUserCommand request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpPost]
    [AllowAnonymous]
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
             new Claim(ClaimTypes.Role , userRoles.Result.First()),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles.Result)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        //Get token
        var token = (await mediator.Send(new GenerateJsonWebTokenQuery { Claims = authClaims }));
        if (token.IsSuccess is false)
            return Unauthorized(token);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token.Result),
            expiration = token.Result?.ValidTo,
            UserName = request.LoginUser.UserName,
        });

    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> FindUser([FromBody] GetUserQuery request)
    {
        return Ok(await mediator.Send(request));
    }
}