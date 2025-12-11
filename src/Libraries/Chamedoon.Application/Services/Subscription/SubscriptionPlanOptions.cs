namespace Chamedoon.Application.Services.Subscription;

public class SubscriptionPlanOptions
{
    public const string SectionName = "Subscription";

    public List<SubscriptionPlanOverride> Plans { get; init; } = new();
}

public class SubscriptionPlanOverride
{
    public string Id { get; init; } = string.Empty;
    public int? Price { get; init; }
    public int? OriginalPrice { get; init; }
}
