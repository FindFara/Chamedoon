using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Dashboard;

public interface IAdminDashboardService
{
    Task<OperationResult<DashboardSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken);
}
