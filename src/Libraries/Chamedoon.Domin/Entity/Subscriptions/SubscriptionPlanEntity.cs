using System;

namespace Chamedoon.Domin.Entity.Subscriptions;

public class SubscriptionPlanEntity
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DurationLabel { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public int OriginalPrice { get; set; }
    public int Price { get; set; }
    public int? EvaluationLimit { get; set; }
    public bool IncludesAI { get; set; }
    public string FeaturesJson { get; set; } = "[]";
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}
