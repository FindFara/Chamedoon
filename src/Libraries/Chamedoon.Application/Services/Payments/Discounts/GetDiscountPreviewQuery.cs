using Chamedoon.Application.Services.Payments;
using MediatR;

namespace Chamedoon.Application.Services.Payments.Discounts;

public record GetDiscountPreviewQuery(string? DiscountCode) : IRequest<DiscountPreviewResult>;

public class GetDiscountPreviewQueryHandler : IRequestHandler<GetDiscountPreviewQuery, DiscountPreviewResult>
{
    private readonly PaymentService _service;

    public GetDiscountPreviewQueryHandler(PaymentService service)
    {
        _service = service;
    }

    public Task<DiscountPreviewResult> Handle(GetDiscountPreviewQuery request, CancellationToken cancellationToken)
        => _service.PreviewDiscountAsync(request.DiscountCode, cancellationToken);
}
