using Chamedoon.Application.Services.Account.Authentication;
using Chamedoon.Application.Services.Account.Command;
using Chamedoon.Application.Services.Account.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        return Ok(await mediator.Send(request));
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
    {
        return Ok(await mediator.Send(request));
    }
}