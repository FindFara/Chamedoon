using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Chamedoon.Application.Services.Subscription;

public static class SubscriptionPlanCatalog
{
    private static readonly object SyncRoot = new();

    private static IReadOnlyList<SubscriptionPlan> _plans = BuildDefaultPlans();

    public static IReadOnlyList<SubscriptionPlan> Plans => _plans;

    public static void Configure(SubscriptionPlanOptions? options)
    {
        lock (SyncRoot)
        {
            _plans = BuildPlans(options);
        }
    }

    public static SubscriptionPlan? Find(string? planId)
        => Plans.FirstOrDefault(plan => plan.Id.Equals(planId ?? string.Empty, StringComparison.OrdinalIgnoreCase));

    private static IReadOnlyList<SubscriptionPlan> BuildPlans(SubscriptionPlanOptions? options)
    {
        var defaults = BuildDefaultPlans();

        if (options?.Plans is null || options.Plans.Count == 0)
        {
            return defaults;
        }

        return defaults
            .Select(plan =>
            {
                var overridePlan = options.Plans.FirstOrDefault(p =>
                    p.Id.Equals(plan.Id, StringComparison.OrdinalIgnoreCase));

                return overridePlan is null
                    ? plan
                    : plan with
                    {
                        Price = overridePlan.Price ?? plan.Price,
                        OriginalPrice = overridePlan.OriginalPrice ?? plan.OriginalPrice
                    };
            })
            .ToList();
    }

    private static IReadOnlyList<SubscriptionPlan> BuildDefaultPlans() => new List<SubscriptionPlan>
    {
        new()
        {
            Id = "starter",
            Title = "پلن پایه (۳ استعلام)",
            DurationLabel = "یک ماهه",
            OriginalPrice = 120_000,
            Price = 37_000,
            EvaluationLimit = 3,
            IncludesAI = false,
            Features = new[]
            {
                "۳ استعلام دقیق ارزیابی مهاجرت",
                "نمایش گزارش کامل در داشبورد",
                "پشتیبانی ایمیلی در تمام مدت اشتراک"
            }
        },
        new()
        {
            Id = "unlimited",
            Title = "پلن حرفه‌ای (نامحدود)",
            DurationLabel = "یک ماهه",
            OriginalPrice = 170_000,
            Price = 49_000,
            EvaluationLimit = null,
            IncludesAI = false,
            Features = new[]
            {
                "استعلام نامحدود در دوره اشتراک",
                "به‌روزرسانی لحظه‌ای مسیرهای مهاجرتی",
                "پشتیبانی سریع‌تر در ساعات اداری"
            }
        },
        new()
        {
            Id = "ai_pro",
            Title = "پلن ویژه (هوش مصنوعی)",
            DurationLabel = "یک ماهه",
            OriginalPrice = 220_000,
            Price = 62_000,
            EvaluationLimit = null,
            IncludesAI = true,
            Features = new[]
            {
                "استعلام نامحدود با دقت بالا",
                "تحلیل پیشرفته با کمک هوش مصنوعی",
                "اولویت در پردازش و پاسخگویی"
            }
        }
    };
}

public class SubscriptionService
{
    private readonly IApplicationDbContext _context;

    public SubscriptionService(IApplicationDbContext context, IOptions<SubscriptionPlanOptions>? planOptions = null)
    {
        _context = context;

        SubscriptionPlanCatalog.Configure(planOptions?.Value);
    }

    public Task<IReadOnlyList<SubscriptionPlan>> GetPlansAsync() => Task.FromResult(SubscriptionPlanCatalog.Plans);

    public async Task<DiscountPreviewResult> PreviewDiscountAsync(string? code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return DiscountPreviewResult.Invalid("کد تخفیف را وارد کن.");
        }

        var discount = await _context.DiscountCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Code == code.Trim(), cancellationToken);

        if (discount is null || !discount.IsActive || (discount.ExpiresAtUtc.HasValue && discount.ExpiresAtUtc.Value <= DateTime.UtcNow))
        {
            return DiscountPreviewResult.Invalid("کد تخفیف معتبر نیست یا منقضی شده است.");
        }

        var plans = SubscriptionPlanCatalog.Plans
            .Select(plan =>
            {
                var discountAmount = CalculateDiscount(plan.Price, discount);
                var finalPrice = Math.Max(1, plan.Price - discountAmount);

                return new DiscountedPlanPrice(plan.Id, plan.Price, plan.OriginalPrice, finalPrice, discountAmount);
            })
            .ToList();

        return DiscountPreviewResult.Valid(discount.Code, plans, "کد تخفیف اعمال شد.");
    }

    public SubscriptionPlan? FindPlan(string? planId) => SubscriptionPlanCatalog.Find(planId);

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

        var plan = FindPlan(customer.SubscriptionPlanId);
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
        var userId = GetUserId(user);
        if (!userId.HasValue)
        {
            return;
        }

        await ActivatePlanForUserAsync(userId.Value, planId, cancellationToken);
    }

    public async Task ActivatePlanForUserAsync(long userId, string planId, CancellationToken cancellationToken)
    {
        var plan = FindPlan(planId);
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
        var plan = FindPlan(customer?.SubscriptionPlanId);

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

    public int RemainingDays => Math.Max(0, (int)Math.Ceiling((EndDateUtc - DateTime.UtcNow).TotalDays));
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
