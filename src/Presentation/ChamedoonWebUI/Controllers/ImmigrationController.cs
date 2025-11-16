using System.Threading.Tasks;
using System.Text.Json;
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
        if (TempData.TryGetValue("ImmigrationInput", out var rawInput) && rawInput is string json)
        {
            try
            {
                var restored = JsonSerializer.Deserialize<ImmigrationInput>(json);
                if (restored is not null)
                {
                    TempData.Keep("ImmigrationInput");
                    return View(restored);
                }
            }
            catch (JsonException)
            {
                // fallback to default model
            }
        }

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

        TempData["ImmigrationInput"] = JsonSerializer.Serialize(input);

        var result = await _mediator.Send(new ImmigrationQuery { Input = input });
        return View("Result", result);
    }
}