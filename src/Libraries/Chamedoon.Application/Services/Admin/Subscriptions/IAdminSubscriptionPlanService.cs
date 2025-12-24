using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Subscriptions;

public interface IAdminSubscriptionPlanService
{
    Task<OperationResult<PaginatedList<AdminSubscriptionPlanDto>>> GetPlansAsync(string? search, int page, int pageSize, CancellationToken cancellationToken);
    Task<OperationResult<AdminSubscriptionPlanDto>> GetPlanAsync(string id, CancellationToken cancellationToken);
    Task<OperationResult<AdminSubscriptionPlanDto>> CreatePlanAsync(AdminSubscriptionPlanInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminSubscriptionPlanDto>> UpdatePlanAsync(AdminSubscriptionPlanInput input, CancellationToken cancellationToken);
    Task<OperationResult<bool>> DeactivatePlanAsync(string id, CancellationToken cancellationToken);
}
