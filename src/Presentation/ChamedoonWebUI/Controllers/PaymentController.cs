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
    public async Task<IActionResult> Callback(long? paymentId, string? trackId, CancellationToken cancellationToken)
    {
        if (!paymentId.HasValue || string.IsNullOrWhiteSpace(trackId))
        {
            TempData["SubscriptionMessage"] = "اطلاعات پرداخت ناقص است.";
            return RedirectToAction("Index", "Subscription");
        }

        var verification = await _mediator.Send(new VerifyPaymentCommand(User, paymentId.Value, trackId), cancellationToken);

        TempData["SubscriptionMessage"] = verification.IsSuccess
            ? "پرداخت با موفقیت انجام شد و اشتراک شما فعال شد."
            : verification.Message ?? "پرداخت تایید نشد.";

        return RedirectToAction("Index", "Subscription");
    }
}

