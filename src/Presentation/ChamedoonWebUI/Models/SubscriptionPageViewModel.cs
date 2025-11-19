using System.Collections.Generic;
using Chamedoon.Application.Services.Subscription;

namespace ChamedoonWebUI.Models;

public class SubscriptionPageViewModel
{
    public IReadOnlyList<SubscriptionPlan> Plans { get; set; } = new List<SubscriptionPlan>();
    public SubscriptionStatus? CurrentSubscription { get; set; }
    public string? AlertMessage { get; set; }
    public bool? LimitReached { get; set; }
}
