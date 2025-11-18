using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Chamedoon.Application.Services.Subscription;

public class SubscriptionMemoryStore
{
    private readonly IReadOnlyList<SubscriptionPlan> _plans;
    private readonly ConcurrentDictionary<string, SubscriptionStatus> _subscriptions;

    public SubscriptionMemoryStore()
    {
        _plans = new List<SubscriptionPlan>
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

        _subscriptions = new ConcurrentDictionary<string, SubscriptionStatus>();
    }

    public IReadOnlyList<SubscriptionPlan> GetPlans() => _plans;

    public SubscriptionStatus? GetCurrentSubscription(ClaimsPrincipal user)
    {
        var userKey = GetUserKey(user);
        if (string.IsNullOrWhiteSpace(userKey))
        {
            return null;
        }

        if (_subscriptions.TryGetValue(userKey, out var status))
        {
            if (status.EndDateUtc <= DateTime.UtcNow)
            {
                _subscriptions.TryRemove(userKey, out _);
                return null;
            }

            var plan = _plans.FirstOrDefault(p => p.Id == status.PlanId);
            if (plan is null)
            {
                return null;
            }

            status.PlanTitle = plan.Title;
            status.IncludesAI = plan.IncludesAI;
            status.EvaluationLimit = plan.EvaluationLimit;
            return status;
        }

        return null;
    }

    public SubscriptionCheckResult CheckEligibility(ClaimsPrincipal user)
    {
        var result = new SubscriptionCheckResult
        {
            IsAuthenticated = user?.Identity?.IsAuthenticated == true
        };

        if (!result.IsAuthenticated)
        {
            return result;
        }

        result.CurrentSubscription = GetCurrentSubscription(user);
        result.HasActiveSubscription = result.CurrentSubscription != null;

        if (result.CurrentSubscription is { EvaluationLimit: not null } subscription)
        {
            result.IsLimitReached = subscription.UsedEvaluations >= subscription.EvaluationLimit;
        }

        return result;
    }

    public void ActivatePlan(ClaimsPrincipal user, string planId)
    {
        var plan = _plans.FirstOrDefault(p => p.Id.Equals(planId, StringComparison.OrdinalIgnoreCase));
        var userKey = GetUserKey(user);

        if (plan is null || string.IsNullOrWhiteSpace(userKey))
        {
            return;
        }

        var status = new SubscriptionStatus
        {
            PlanId = plan.Id,
            PlanTitle = plan.Title,
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddMonths(1),
            EvaluationLimit = plan.EvaluationLimit,
            IncludesAI = plan.IncludesAI,
            UsedEvaluations = 0
        };

        _subscriptions.AddOrUpdate(userKey, status, (_, _) => status);
    }

    public void RegisterImmigrationUsage(ClaimsPrincipal user)
    {
        var userKey = GetUserKey(user);
        if (string.IsNullOrWhiteSpace(userKey))
        {
            return;
        }

        if (_subscriptions.TryGetValue(userKey, out var status) && status.EvaluationLimit is not null)
        {
            status.UsedEvaluations++;
        }
    }

    private static string? GetUserKey(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.Identity?.Name;
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
