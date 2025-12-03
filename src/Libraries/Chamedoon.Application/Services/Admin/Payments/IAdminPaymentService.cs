using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Admin.Payments;

public interface IAdminPaymentService
{
    Task<OperationResult<PaginatedList<AdminPaymentDto>>> GetPaymentsAsync(
        string? search,
        PaymentStatus? status,
        DateTime? fromDate,
        DateTime? toDate,
        string? userName,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
