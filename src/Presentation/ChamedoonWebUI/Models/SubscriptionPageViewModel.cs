using System.Collections.Generic;
using Chamedoon.Application.Services.Subscription;

namespace ChamedoonWebUI.Models;

public class SubscriptionPageViewModel
{
    public IReadOnlyList<SubscriptionPlan> Plans { get; set; } = new List<SubscriptionPlan>();
    public SubscriptionStatus? CurrentSubscription { get; set; }
    public string? AlertMessage { get; set; }
    public bool? LimitReached { get; set; }
    public string? DiscountCode { get; set; }
    public string? PaymentMessage { get; set; }
    public bool? PaymentSuccess { get; set; }
    public string? ReturnUrl { get; set; }
}
