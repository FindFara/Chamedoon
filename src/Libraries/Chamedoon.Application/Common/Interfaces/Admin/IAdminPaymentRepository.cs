using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminPaymentRepository
{
    Task<DashboardPaymentSummaryDto> GetPaymentSummaryAsync(DateTime since, CancellationToken cancellationToken);
    Task<IReadOnlyList<DashboardPaymentActivityDto>> GetRecentPaymentsAsync(int count, CancellationToken cancellationToken);
    Task<PaginatedList<PaymentRequest>> GetPaymentsAsync(string? search, PaymentStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken);
}

