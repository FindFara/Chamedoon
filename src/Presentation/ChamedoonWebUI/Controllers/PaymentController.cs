using System;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Services.Payments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Controllers;

[Route("payment")]
public class PaymentController : Controller
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("callback")]
    [AllowAnonymous]
    public async Task<IActionResult> Callback(long? paymentId, string? trackId, string? returnUrl, CancellationToken cancellationToken)
    {
        var resolvedReturnUrl = NormalizeReturnUrl(returnUrl) ?? NormalizeReturnUrl(TempData.Peek("SubscriptionReturnUrl") as string);

        if (!string.IsNullOrWhiteSpace(resolvedReturnUrl))
        {
            TempData["SubscriptionReturnUrl"] = resolvedReturnUrl;
        }

        if (!paymentId.HasValue || string.IsNullOrWhiteSpace(trackId))
        {
            TempData["SubscriptionMessage"] = "اطلاعات پرداخت ناقص است.";
            TempData["SubscriptionSuccess"] = false;
            return RedirectToAction("Index", "Subscription", new { returnUrl = resolvedReturnUrl });
        }

        var verification = await _mediator.Send(new VerifyPaymentCommand(User, paymentId.Value, trackId), cancellationToken);

        TempData["SubscriptionSuccess"] = verification.IsSuccess;
        TempData["SubscriptionMessage"] = verification.IsSuccess
            ? "پرداخت با موفقیت انجام شد و اشتراک شما فعال شد."
            : "تراکنش با خطا مواجه شد.";

        return RedirectToAction("Index", "Subscription", new { returnUrl = resolvedReturnUrl });
    }

    private string? NormalizeReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return null;
        }

        if (Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        if (Uri.TryCreate(returnUrl, UriKind.Absolute, out var absoluteUri))
        {
            var localPath = absoluteUri.PathAndQuery;
            if (Url.IsLocalUrl(localPath))
            {
                return localPath;
            }
        }

        return null;
    }
}
