using System;
using System.Threading;
using System.Threading.Tasks;
using ChamedoonWebUI.Models;
using Chamedoon.Application.Services.Subscription;
using Chamedoon.Application.Services.Payments;
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
        var tempDiscount = TempData["SubscriptionDiscountCode"] as string;

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
            LimitReached = string.Equals(reason, "limit", StringComparison.OrdinalIgnoreCase),
            DiscountCode = tempDiscount
        };

        ViewData["Title"] = "انتخاب اشتراک";
        return View(model);
    }

    [HttpPost("subscribe")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(string planId, string? discountCode)
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

        var callbackUrl = Url.Action("Callback", "Payment", values: null, protocol: Request.Scheme);
        if (string.IsNullOrWhiteSpace(callbackUrl))
        {
            TempData["SubscriptionMessage"] = "آدرس بازگشت پرداخت تنظیم نشده است.";
            return RedirectToAction(nameof(Index));
        }

        var paymentResult = await _mediator.Send(new StartSubscriptionPaymentCommand(User, planId, callbackUrl, discountCode));
        if (!paymentResult.IsSuccess || string.IsNullOrWhiteSpace(paymentResult.RedirectUrl))
        {
            TempData["SubscriptionMessage"] = paymentResult.ErrorMessage ?? "خطایی در شروع فرآیند پرداخت رخ داد.";
            TempData["SubscriptionDiscountCode"] = discountCode;
            return RedirectToAction(nameof(Index));
        }

        return Redirect(paymentResult.RedirectUrl);
    }

    [HttpPost("preview-discount")]
    public async Task<IActionResult> PreviewDiscount([FromBody] DiscountPreviewRequest request, CancellationToken cancellationToken)
    {
        var preview = await _mediator.Send(new GetDiscountPreviewQuery(request.Code), cancellationToken);
        return Ok(preview);
    }
}

public record DiscountPreviewRequest(string? Code);
