using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Chamedoon.Application.Services.Immigration;
using Chamedoon.Application.Services.Subscription;
using MediatR;

namespace ChamedoonWebUI.Controllers;

public class ImmigrationController : Controller
{
    private readonly IMediator _mediator;
    private readonly IImmigrationEvaluationService _evaluationService;

    public ImmigrationController(IMediator mediator, IImmigrationEvaluationService evaluationService)
    {
        _mediator = mediator;
        _evaluationService = evaluationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var eligibilityRedirect = await ValidateEligibilityAsync();
        if (eligibilityRedirect is not null)
        {
            return eligibilityRedirect;
        }

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
        var eligibilityRedirect = await ValidateEligibilityAsync();
        if (eligibilityRedirect is not null)
        {
            return eligibilityRedirect;
        }

        if (!ModelState.IsValid)
        {
            return View("Index", input);
        }

        TempData["ImmigrationInput"] = JsonSerializer.Serialize(input);

        await _mediator.Send(new RegisterSubscriptionUsageCommand(User));
        await _evaluationService.RecordAsync(input, User, HttpContext.RequestAborted);
        // var result = await _mediator.Send(new ImmigrationQuery { Input = input });
        var result = await _mediator.Send(new ImmigrationAiQuery { Input = input });
        return View("Result", result);
    }

    private async Task<IActionResult?> ValidateEligibilityAsync()
    {
        var eligibility = await _mediator.Send(new CheckSubscriptionEligibilityQuery(User));

        if (!eligibility.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account", new
            {
                area = string.Empty,
                returnUrl = Url.Action("Index", "Immigration")
            });
        }

        if (!eligibility.HasActiveSubscription)
        {
            TempData["SubscriptionMessage"] = "برای شروع ارزیابی، لازم است اشتراک فعال داشته باشی.";
            return RedirectToAction("Index", "Subscription", new
            {
                reason = "needsPlan",
                returnUrl = Url.Action("Index", "Immigration")
            });
        }

        if (eligibility.IsLimitReached)
        {
            TempData["SubscriptionMessage"] = "تعداد استعلام مجاز این ماه تمام شده. می‌توانی اشتراک نامحدود بگیری.";
            return RedirectToAction("Index", "Subscription", new
            {
                reason = "limit",
                returnUrl = Url.Action("Index", "Immigration")
            });
        }

        return null;
    }
}
