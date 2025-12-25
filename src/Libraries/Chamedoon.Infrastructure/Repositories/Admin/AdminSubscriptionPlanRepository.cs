using System;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Subscriptions;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminSubscriptionPlanRepository : IAdminSubscriptionPlanRepository
{
    private readonly ApplicationDbContext _context;

    public AdminSubscriptionPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<PaginatedList<SubscriptionPlanEntity>> GetPlansAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.SubscriptionPlans.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.Trim().ToLower();
            query = query.Where(plan =>
                (!string.IsNullOrEmpty(plan.Id) && plan.Id.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(plan.Title) && plan.Title.ToLower().Contains(lowered)));
        }

        query = query.OrderBy(plan => plan.SortOrder).ThenBy(plan => plan.Title);
        return PaginatedList<SubscriptionPlanEntity>.CreateAsync(query, pageNumber, pageSize);
    }

    public Task<SubscriptionPlanEntity?> GetPlanAsync(string id, CancellationToken cancellationToken)
        => _context.SubscriptionPlans.AsNoTracking().FirstOrDefaultAsync(plan => plan.Id == id, cancellationToken);

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
        => _context.SubscriptionPlans.AnyAsync(plan => plan.Id == id, cancellationToken);

    public async Task<SubscriptionPlanEntity> CreatePlanAsync(SubscriptionPlanEntity plan, CancellationToken cancellationToken)
    {
        plan.CreatedAtUtc = DateTime.Now;
        _context.SubscriptionPlans.Add(plan);
        await _context.SaveChangesAsync(cancellationToken);
        return plan;
    }

    public async Task<SubscriptionPlanEntity?> UpdatePlanAsync(SubscriptionPlanEntity plan, CancellationToken cancellationToken)
    {
        var existing = await _context.SubscriptionPlans.FirstOrDefaultAsync(item => item.Id == plan.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.Title = plan.Title;
        existing.DurationLabel = plan.DurationLabel;
        existing.OriginalPrice = plan.OriginalPrice;
        existing.Price = plan.Price;
        existing.EvaluationLimit = plan.EvaluationLimit;
        existing.IncludesAI = plan.IncludesAI;
        existing.FeaturesJson = plan.FeaturesJson;
        existing.IsActive = plan.IsActive;
        existing.SortOrder = plan.SortOrder;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeactivatePlanAsync(string id, CancellationToken cancellationToken)
    {
        var existing = await _context.SubscriptionPlans.FirstOrDefaultAsync(plan => plan.Id == id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
