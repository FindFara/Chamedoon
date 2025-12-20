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
    public async Task<IActionResult> Index(string? reason = null, string? returnUrl = null)
    {
        var subscriptionMessage = TempData["SubscriptionMessage"] as string;
        var tempDiscount = TempData["SubscriptionDiscountCode"] as string;
        var paymentSuccess = ReadTempDataBool("SubscriptionSuccess");
        var resolvedReturnUrl = NormalizeReturnUrl(returnUrl)
            ?? NormalizeReturnUrl(TempData.Peek("SubscriptionReturnUrl") as string)
            ?? NormalizeReturnUrl(Request.Headers["Referer"]);

        if (!string.IsNullOrWhiteSpace(resolvedReturnUrl))
        {
            TempData["SubscriptionReturnUrl"] = resolvedReturnUrl;
        }

        var alertMessage = paymentSuccess.HasValue ? null : subscriptionMessage;
        var paymentMessage = paymentSuccess.HasValue ? subscriptionMessage : null;

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
            DiscountCode = tempDiscount,
            PaymentSuccess = paymentSuccess,
            PaymentMessage = paymentMessage,
            ReturnUrl = resolvedReturnUrl
        };

        ViewData["Title"] = "انتخاب اشتراک";
        return View(model);
    }

    [HttpPost("apply-discount")]
    public async Task<IActionResult> ApplyDiscount(string code, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new PreviewDiscountQuery(code), cancellationToken);

        if (!result.IsValid)
        {
            return BadRequest(new { message = result.Message ?? "کد تخفیف معتبر نیست." });
        }

        return Ok(new
        {
            message = result.Message,
            code = result.AppliedCode,
            plans = result.Plans
        });
    }

    [HttpPost("subscribe")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(string planId, string? discountCode, string? returnUrl)
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

        var resolvedReturnUrl = NormalizeReturnUrl(returnUrl) ?? NormalizeReturnUrl(TempData.Peek("SubscriptionReturnUrl") as string);
        if (!string.IsNullOrWhiteSpace(resolvedReturnUrl))
        {
            TempData["SubscriptionReturnUrl"] = resolvedReturnUrl;
        }

        var callbackUrl = Url.Action("Callback", "Payment", new { returnUrl = resolvedReturnUrl }, Request.Scheme);
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

    private bool? ReadTempDataBool(string key)
    {
        if (!TempData.TryGetValue(key, out var raw) || raw is null)
        {
            return null;
        }

        return raw switch
        {
            bool b => b,
            string s when bool.TryParse(s, out var parsed) => parsed,
            _ => null
        };
    }

    private string? NormalizeReturnUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (Url.IsLocalUrl(value))
        {
            return value;
        }

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            var localPath = uri.PathAndQuery;
            if (Url.IsLocalUrl(localPath))
            {
                return localPath;
            }
        }

        return null;
    }
}
