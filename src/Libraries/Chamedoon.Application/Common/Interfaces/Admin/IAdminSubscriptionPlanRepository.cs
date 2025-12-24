using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Subscriptions;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminSubscriptionPlanRepository
{
    Task<PaginatedList<SubscriptionPlanEntity>> GetPlansAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<SubscriptionPlanEntity?> GetPlanAsync(string id, CancellationToken cancellationToken);
    Task<SubscriptionPlanEntity> CreatePlanAsync(SubscriptionPlanEntity plan, CancellationToken cancellationToken);
    Task<SubscriptionPlanEntity?> UpdatePlanAsync(SubscriptionPlanEntity plan, CancellationToken cancellationToken);
    Task<bool> DeactivatePlanAsync(string id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);
}
