using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chamedoon.Application.Services.Immigration;
using MediatR;

namespace ChamedoonWebUI.Controllers;

public class ImmigrationController : Controller
{
    private readonly IMediator _mediator;
    public ImmigrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ImmigrationInput());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Calculate(ImmigrationInput input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", input);
        }
        var result = await _mediator.Send(new ImmigrationQuery { Input = input });
        return View("Result", result);
    }
}