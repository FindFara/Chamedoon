using System.Security.Claims;
using MediatR;

namespace Chamedoon.Application.Services.Payments;

public record StartSubscriptionPaymentCommand(ClaimsPrincipal User, string PlanId, string CallbackUrl, string? DiscountCode) : IRequest<PaymentRedirectResult>;

public class StartSubscriptionPaymentCommandHandler : IRequestHandler<StartSubscriptionPaymentCommand, PaymentRedirectResult>
{
    private readonly PaymentService _service;

    public StartSubscriptionPaymentCommandHandler(PaymentService service)
    {
        _service = service;
    }

    public Task<PaymentRedirectResult> Handle(StartSubscriptionPaymentCommand request, CancellationToken cancellationToken)
        => _service.StartSubscriptionPaymentAsync(request.User, request.PlanId, request.CallbackUrl, request.DiscountCode, cancellationToken);
}

public record VerifyPaymentCommand(ClaimsPrincipal? User, long PaymentRequestId, string TrackId) : IRequest<PaymentVerificationResult>;

public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, PaymentVerificationResult>
{
    private readonly PaymentService _service;

    public VerifyPaymentCommandHandler(PaymentService service)
    {
        _service = service;
    }

    public Task<PaymentVerificationResult> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        => _service.VerifyPaymentAsync(request.User, request.PaymentRequestId, request.TrackId, cancellationToken);
}

