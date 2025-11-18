using System.Security.Claims;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Enums;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Subscription;

public static class SubscriptionPlanCatalog
{
    public static readonly IReadOnlyList<SubscriptionPlan> Plans = new List<SubscriptionPlan>
    {
        new()
        {
            Id = "starter",
            Title = "اشتراک پایه",
            DurationLabel = "یک ماهه",
            Price = 120_000,
            EvaluationLimit = 3,
            IncludesAI = false,
            Features = new[]
            {
                "۳ استعلام مهاجرتی دقیق",
                "دسترسی به داشبورد و ذخیره‌سازی نتایج",
                "پشتیبانی ایمیلی"
            }
        },
        new()
        {
            Id = "unlimited",
            Title = "اشتراک نامحدود",
            DurationLabel = "یک ماهه",
            Price = 170_000,
            EvaluationLimit = null,
            IncludesAI = false,
            Features = new[]
            {
                "استعلام نامحدود در طول ماه",
                "آپدیت لحظه‌ای مسیرهای مهاجرتی",
                "پشتیبانی ویژه"
            }
        },
        new()
        {
            Id = "ai_pro",
            Title = "هوش مصنوعی + نامحدود",
            DurationLabel = "یک ماهه",
            Price = 220_000,
            EvaluationLimit = null,
            IncludesAI = true,
            Features = new[]
            {
                "استعلام نامحدود",
                "بهینه‌سازی پاسخ‌ها با هوش مصنوعی",
                "تحلیل سریع‌تر برای پرونده‌های پیچیده"
            }
        }
    };

    public static SubscriptionPlan? Find(string? planId)
        => Plans.FirstOrDefault(plan => plan.Id.Equals(planId ?? string.Empty, StringComparison.OrdinalIgnoreCase));
}

public class SubscriptionService
{
    private readonly IApplicationDbContext _context;

    public SubscriptionService(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<SubscriptionPlan>> GetPlansAsync() => Task.FromResult(SubscriptionPlanCatalog.Plans);

    public async Task<SubscriptionStatus?> GetCurrentSubscriptionAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var userId = GetUserId(user);
        if (!userId.HasValue)
        {
            return null;
        }

        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == userId.Value, cancellationToken);

        if (customer is null || string.IsNullOrWhiteSpace(customer.SubscriptionPlanId))
        {
            return null;
        }

        if (customer.SubscriptionEndDateUtc.HasValue && customer.SubscriptionEndDateUtc.Value <= DateTime.UtcNow)
        {
            return null;
        }

        var plan = SubscriptionPlanCatalog.Find(customer.SubscriptionPlanId);
        if (plan is null)
        {
            return null;
        }

        return new SubscriptionStatus
        {
            PlanId = plan.Id,
            PlanTitle = plan.Title,
            IncludesAI = plan.IncludesAI,
            EvaluationLimit = plan.EvaluationLimit,
            UsedEvaluations = customer.UsedEvaluations,
            StartDateUtc = customer.SubscriptionStartDateUtc ?? DateTime.UtcNow,
            EndDateUtc = customer.SubscriptionEndDateUtc ?? DateTime.UtcNow
        };
    }

    public async Task<SubscriptionCheckResult> CheckEligibilityAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var result = new SubscriptionCheckResult
        {
            IsAuthenticated = user?.Identity?.IsAuthenticated == true
        };

        if (!result.IsAuthenticated)
        {
            return result;
        }

        result.CurrentSubscription = await GetCurrentSubscriptionAsync(user, cancellationToken);
        result.HasActiveSubscription = result.CurrentSubscription != null;

        if (result.CurrentSubscription is { EvaluationLimit: not null } subscription)
        {
            result.IsLimitReached = subscription.UsedEvaluations >= subscription.EvaluationLimit;
        }

        return result;
    }

    public async Task ActivatePlanAsync(ClaimsPrincipal user, string planId, CancellationToken cancellationToken)
    {
        var plan = SubscriptionPlanCatalog.Find(planId);
        var userId = GetUserId(user);

        if (plan is null || !userId.HasValue)
        {
            return;
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId.Value, cancellationToken);
        if (customer is null)
        {
            var account = await _context.User.FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken);
            if (account is null)
            {
                return;
            }

            customer = new Customer
            {
                Id = account.Id,
                User = account,
                Gender = Gender.Unknown
            };

            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        customer.SubscriptionPlanId = plan.Id;
        customer.SubscriptionStartDateUtc = DateTime.UtcNow;
        customer.SubscriptionEndDateUtc = DateTime.UtcNow.AddMonths(1);
        customer.UsedEvaluations = 0;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RegisterImmigrationUsageAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var userId = GetUserId(user);
        if (!userId.HasValue)
        {
            return;
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId.Value, cancellationToken);
        var plan = SubscriptionPlanCatalog.Find(customer?.SubscriptionPlanId);

        if (customer is null || plan is null || plan.EvaluationLimit is null)
        {
            return;
        }

        if (customer.SubscriptionEndDateUtc.HasValue && customer.SubscriptionEndDateUtc.Value <= DateTime.UtcNow)
        {
            return;
        }

        customer.UsedEvaluations = Math.Min(customer.UsedEvaluations + 1, plan.EvaluationLimit.Value);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static long? GetUserId(ClaimsPrincipal user)
    {
        var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.Identity?.Name;
        return long.TryParse(idValue, out var id) ? id : null;
    }
}

public record SubscriptionPlan
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string DurationLabel { get; init; } = string.Empty;
    public int Price { get; init; }
    public int? EvaluationLimit { get; init; }
    public bool IncludesAI { get; init; }
    public IReadOnlyList<string> Features { get; init; } = Array.Empty<string>();
}

public record SubscriptionStatus
{
    public string PlanId { get; set; } = string.Empty;
    public string PlanTitle { get; set; } = string.Empty;
    public bool IncludesAI { get; set; }
    public int? EvaluationLimit { get; set; }
    public int UsedEvaluations { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }

    public int RemainingDays => Math.Max(0, (int)Math.Ceiling((EndDateUtc - DateTime.UtcNow).TotalDays));
}

public record SubscriptionCheckResult
{
    public bool IsAuthenticated { get; set; }
    public bool HasActiveSubscription { get; set; }
    public bool IsLimitReached { get; set; }
    public SubscriptionStatus? CurrentSubscription { get; set; }
}
