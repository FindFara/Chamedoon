using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Admin.Payments;

public class AdminPaymentService : IAdminPaymentService
{
    private readonly IAdminPaymentRepository _paymentRepository;

    public AdminPaymentService(IAdminPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<OperationResult<PaginatedList<AdminPaymentDto>>> GetPaymentsAsync(
        string? search,
        PaymentStatus? status,
        DateTime? fromDate,
        DateTime? toDate,
        string? userName,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetPaymentsAsync(
            search,
            status,
            fromDate,
            toDate,
            userName,
            pageNumber,
            pageSize,
            cancellationToken);
        var mappedItems = payments.Items.Select(payment => payment.ToAdminPaymentDto()).ToList();
        var paginated = new PaginatedList<AdminPaymentDto>(mappedItems, payments.TotalCount, payments.PageNumber, pageSize);

        return OperationResult<PaginatedList<AdminPaymentDto>>.Success(paginated);
    }
}
