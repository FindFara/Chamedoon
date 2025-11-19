using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminPaymentRepository
{
    Task<DashboardPaymentSummaryDto> GetPaymentSummaryAsync(DateTime since, CancellationToken cancellationToken);
    Task<IReadOnlyList<DashboardPaymentActivityDto>> GetRecentPaymentsAsync(int count, CancellationToken cancellationToken);
}

