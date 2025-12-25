using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Entity.Subscriptions;
using Chamedoon.Domin.Enums;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Subscription;

public class SubscriptionService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IApplicationDbContext _context;

    public SubscriptionService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SubscriptionPlan>> GetPlansAsync(CancellationToken cancellationToken, bool includeInactive = false)
    {
        var query = _context.SubscriptionPlans.AsNoTracking();
        if (!includeInactive)
        {
            query = query.Where(plan => plan.IsActive);
        }

        var plans = await query
            .OrderBy(plan => plan.SortOrder)
            .ThenBy(plan => plan.Title)
            .ToListAsync(cancellationToken);

        return plans.Select(MapPlan).ToList();
    }

    public async Task<SubscriptionPlan?> FindPlanAsync(string? planId, CancellationToken cancellationToken, bool includeInactive = true)
    {
        if (string.IsNullOrWhiteSpace(planId))
        {
            return null;
        }

        var query = _context.SubscriptionPlans.AsNoTracking().Where(plan => plan.Id == planId);
        if (!includeInactive)
        {
            query = query.Where(plan => plan.IsActive);
        }

        var plan = await query.FirstOrDefaultAsync(cancellationToken);
        return plan is null ? null : MapPlan(plan);
    }

    public async Task<IReadOnlyDictionary<string, string>> GetPlanTitleLookupAsync(CancellationToken cancellationToken)
    {
        var plans = await _context.SubscriptionPlans
            .AsNoTracking()
            .Select(plan => new { plan.Id, plan.Title })
            .ToListAsync(cancellationToken);

        return plans.ToDictionary(plan => plan.Id, plan => plan.Title);
    }

    public async Task<DiscountPreviewResult> PreviewDiscountAsync(string? code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return DiscountPreviewResult.Invalid("کد تخفیف را وارد کن.");
        }

        var normalizedCode = NormalizeCode(code);

        var discount = await _context.DiscountCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Code.ToLower() == normalizedCode, cancellationToken);

        if (discount is null || !discount.IsActive || (discount.ExpiresAtUtc.HasValue && discount.ExpiresAtUtc.Value <= DateTime.Now))
        {
            return DiscountPreviewResult.Invalid("کد تخفیف معتبر نیست یا منقضی شده است.");
        }

        var plans = await GetPlansAsync(cancellationToken);
        var discountedPlans = plans
            .Select(plan =>
            {
                var discountAmount = CalculateDiscount(plan.Price, discount);
                var finalPrice = Math.Max(1, plan.Price - discountAmount);

                return new DiscountedPlanPrice(plan.Id, plan.Price, plan.OriginalPrice, finalPrice, discountAmount);
            })
            .ToList();

        return DiscountPreviewResult.Valid(discount.Code, discountedPlans, "کد تخفیف اعمال شد.");
    }

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

        if (customer.SubscriptionEndDateUtc.HasValue && customer.SubscriptionEndDateUtc.Value <= DateTime.Now)
        {
            return null;
        }

        var plan = await FindPlanAsync(customer.SubscriptionPlanId, cancellationToken, includeInactive: true);
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
            StartDateUtc = customer.SubscriptionStartDateUtc ?? DateTime.Now,
            EndDateUtc = customer.SubscriptionEndDateUtc ?? DateTime.Now
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
        var userId = GetUserId(user);
        if (!userId.HasValue)
        {
            return;
        }

        await ActivatePlanForUserAsync(userId.Value, planId, cancellationToken);
    }

    public async Task ActivatePlanForUserAsync(long userId, string planId, CancellationToken cancellationToken)
    {
        var plan = await FindPlanAsync(planId, cancellationToken, includeInactive: false);
        if (plan is null)
        {
            return;
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId, cancellationToken);
        if (customer is null)
        {
            var account = await _context.User.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
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
        customer.SubscriptionStartDateUtc = DateTime.Now;
        var months = plan.DurationMonths <= 0 ? 1 : plan.DurationMonths;
        customer.SubscriptionEndDateUtc = DateTime.Now.AddMonths(months);
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
        var plan = await FindPlanAsync(customer?.SubscriptionPlanId, cancellationToken, includeInactive: true);

        if (customer is null || plan is null || plan.EvaluationLimit is null)
        {
            return;
        }

        if (customer.SubscriptionEndDateUtc.HasValue && customer.SubscriptionEndDateUtc.Value <= DateTime.Now)
        {
            return;
        }

        customer.UsedEvaluations = Math.Min(customer.UsedEvaluations + 1, plan.EvaluationLimit.Value);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static SubscriptionPlan MapPlan(SubscriptionPlanEntity entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            DurationLabel = entity.DurationLabel,
            DurationMonths = entity.DurationMonths,
            OriginalPrice = entity.OriginalPrice,
            Price = entity.Price,
            EvaluationLimit = entity.EvaluationLimit,
            IncludesAI = entity.IncludesAI,
            Features = ParseFeatures(entity.FeaturesJson)
        };

    private static IReadOnlyList<string> ParseFeatures(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<string>();
        }

        try
        {
            var list = JsonSerializer.Deserialize<List<string>>(json, JsonOptions);
            return list?.Where(item => !string.IsNullOrWhiteSpace(item)).ToList() ??new List<string>();
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    private static long? GetUserId(ClaimsPrincipal user)
    {
        var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.Identity?.Name;
        return long.TryParse(idValue, out var id) ? id : null;
    }

    private static string NormalizeCode(string? code) => (code ?? string.Empty).Trim().ToLowerInvariant();

    private static int CalculateDiscount(int amount, DiscountCode? discount)
    {
        if (discount is null || amount <= 0 || discount.Value <= 0)
        {
            return 0;
        }

        var discountAmount = discount.Type == DiscountType.Percentage
            ? (int)Math.Round(amount * discount.Value / 100m)
            : discount.Value;

        return Math.Max(0, Math.Min(amount, discountAmount));
    }
}

public record SubscriptionPlan
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string DurationLabel { get; init; } = string.Empty;
    public int DurationMonths { get; init; }
    public int OriginalPrice { get; init; }
    public int Price { get; init; }
    public int? EvaluationLimit { get; init; }
    public bool IncludesAI { get; init; }
    public IReadOnlyList<string> Features { get; init; } = Array.Empty<string>();

    public int DiscountPercent => OriginalPrice <= 0 || Price <= 0
        ? 0
        : Math.Max(0, 100 - (int)Math.Round((double)Price / OriginalPrice * 100));

    public int SavingsAmount => Math.Max(0, OriginalPrice - Price);
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

    public int RemainingDays => Math.Max(0, (int)Math.Ceiling((EndDateUtc - DateTime.Now).TotalDays));
}

public record SubscriptionCheckResult
{
    public bool IsAuthenticated { get; set; }
    public bool HasActiveSubscription { get; set; }
    public bool IsLimitReached { get; set; }
    public SubscriptionStatus? CurrentSubscription { get; set; }
}

public record DiscountedPlanPrice(string PlanId, int BasePrice, int OriginalPrice, int FinalPrice, int DiscountAmount);

public record DiscountPreviewResult(bool IsValid, string? Message, string? AppliedCode, IReadOnlyList<DiscountedPlanPrice> Plans)
{
    public static DiscountPreviewResult Invalid(string? message)
        => new(false, message, null, Array.Empty<DiscountedPlanPrice>());

    public static DiscountPreviewResult Valid(string appliedCode, IReadOnlyList<DiscountedPlanPrice> plans, string? message)
        => new(true, message, appliedCode, plans);
}
