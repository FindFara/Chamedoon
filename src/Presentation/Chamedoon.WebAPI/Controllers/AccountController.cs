using Chamedoon.Application.Services.Account.Authentication;
using Chamedoon.Application.Services.Account.Command;
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


    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
    {
        return Ok(await mediator.Send(request));
    }
}