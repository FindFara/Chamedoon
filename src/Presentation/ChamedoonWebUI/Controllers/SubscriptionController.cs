using System;
using System.Threading.Tasks;
using ChamedoonWebUI.Models;
using Chamedoon.Application.Services.Subscription;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Controllers;

[Route("subscriptions")]
public class SubscriptionController : Controller
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? reason = null)
    {
        var alertMessage = TempData["SubscriptionMessage"] as string;

        if (!string.IsNullOrWhiteSpace(reason) && string.IsNullOrWhiteSpace(alertMessage))
        {
            alertMessage = reason switch
            {
                "needsPlan" => "برای شروع ارزیابی، یکی از اشتراک‌ها را فعال کن.",
                "limit" => "تعداد استعلام این دوره تمام شده. می‌توانی اشتراک نامحدود را فعال کنی.",
                _ => null
            };
        }

        var model = new SubscriptionPageViewModel
        {
            Plans = await _mediator.Send(new GetSubscriptionPlansQuery()),
            CurrentSubscription = await _mediator.Send(new GetSubscriptionStatusQuery(User)),
            AlertMessage = alertMessage,
            LimitReached = string.Equals(reason, "limit", StringComparison.OrdinalIgnoreCase)
        };

        ViewData["Title"] = "انتخاب اشتراک";
        return View(model);
    }

    [HttpPost("subscribe")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(string planId)
    {
        var eligibility = await _mediator.Send(new CheckSubscriptionEligibilityQuery(User));
        if (!eligibility.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account", new
            {
                area = string.Empty,
                returnUrl = Url.Action("Index", "Subscription")
            });
        }

        await _mediator.Send(new ActivateSubscriptionCommand(User, planId));
        TempData["SubscriptionMessage"] = "اشتراک شما فعال شد و آماده شروع ارزیابی هستی.";
        return RedirectToAction(nameof(Index));
    }
}
