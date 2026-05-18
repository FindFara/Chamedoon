using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Enums;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class DashboardViewModel
{
    public int TotalUsers { get; init; }
    public int ActiveUsers { get; init; }
    public int NewUsersThisMonth { get; init; }
    public int TotalBlogPosts { get; init; }
    public int PublishedBlogPosts { get; init; }
    public int DraftBlogPosts { get; init; }
    public long TotalViews { get; init; }
    public int UsersWithActiveSubscription { get; init; }
    public IReadOnlyList<DashboardPopularPostViewModel> PopularPosts { get; init; } = Array.Empty<DashboardPopularPostViewModel>();
    public IReadOnlyList<DashboardRoleDistributionViewModel> RoleDistribution { get; init; } = Array.Empty<DashboardRoleDistributionViewModel>();
    public IReadOnlyList<DashboardPermissionUsageViewModel> PermissionUsage { get; init; } = Array.Empty<DashboardPermissionUsageViewModel>();
    public IReadOnlyList<DashboardMonthlyRegistrationViewModel> MonthlyRegistrations { get; init; } = Array.Empty<DashboardMonthlyRegistrationViewModel>();
    public IReadOnlyList<DashboardMonthlyRegistrationViewModel> MonthlyActiveSubscriptions { get; init; } = Array.Empty<DashboardMonthlyRegistrationViewModel>();
    public IReadOnlyList<DashboardMonthlyRegistrationViewModel> MonthlyBlogViews { get; init; } = Array.Empty<DashboardMonthlyRegistrationViewModel>();
    public IReadOnlyList<DashboardDailyRegistrationViewModel> DailyRegistrationsLast30Days { get; init; } = Array.Empty<DashboardDailyRegistrationViewModel>();
    public IReadOnlyList<DashboardDailyRegistrationViewModel> DailyPaidSubscriptionsLast30Days { get; init; } = Array.Empty<DashboardDailyRegistrationViewModel>();
    public IReadOnlyList<DashboardSubscriptionPlanPurchaseViewModel> DailySubscriptionPlanPurchases { get; init; } = Array.Empty<DashboardSubscriptionPlanPurchaseViewModel>();
    public IReadOnlyList<DashboardSubscriptionPlanPurchaseViewModel> YesterdaySubscriptionPlanPurchases { get; init; } = Array.Empty<DashboardSubscriptionPlanPurchaseViewModel>();
    public IReadOnlyList<DashboardSubscriptionPlanPurchaseViewModel> MonthlySubscriptionPlanPurchases { get; init; } = Array.Empty<DashboardSubscriptionPlanPurchaseViewModel>();
    public IReadOnlyList<UserListItemViewModel> RecentUsers { get; init; } = Array.Empty<UserListItemViewModel>();
    public IReadOnlyList<BlogListItemViewModel> RecentPosts { get; init; } = Array.Empty<BlogListItemViewModel>();
    public DashboardPaymentSummaryViewModel PaymentSummary { get; init; } = new();
    public IReadOnlyList<DashboardPaymentActivityViewModel> RecentPayments { get; init; } = Array.Empty<DashboardPaymentActivityViewModel>();

    public static DashboardViewModel FromDto(DashboardSummaryDto dto)
    {
        return new DashboardViewModel
        {
            TotalUsers = dto.TotalUsers,
            ActiveUsers = dto.ActiveUsers,
            NewUsersThisMonth = dto.NewUsersThisMonth,
            TotalBlogPosts = dto.TotalBlogPosts,
            PublishedBlogPosts = dto.PublishedBlogPosts,
            DraftBlogPosts = dto.DraftBlogPosts,
            TotalViews = dto.TotalViews,
            UsersWithActiveSubscription = dto.UsersWithActiveSubscription,
            PopularPosts = dto.PopularPosts.Select(item => new DashboardPopularPostViewModel(item.Title, item.VisitCount)).ToList(),
            RoleDistribution = dto.RoleDistribution.Select(item => new DashboardRoleDistributionViewModel(item.RoleName, item.UserCount)).ToList(),
            PermissionUsage = dto.PermissionUsage.Select(item => new DashboardPermissionUsageViewModel(item.PermissionName, item.RoleCount)).ToList(),
            MonthlyRegistrations = dto.MonthlyRegistrations.Select(item => new DashboardMonthlyRegistrationViewModel(item.Month, item.Count)).ToList(),
            MonthlyActiveSubscriptions = dto.MonthlyActiveSubscriptions.Select(item => new DashboardMonthlyRegistrationViewModel(item.Month, item.Count)).ToList(),
            MonthlyBlogViews = dto.MonthlyBlogViews.Select(item => new DashboardMonthlyRegistrationViewModel(item.Month, item.Count)).ToList(),
            DailyRegistrationsLast30Days = dto.DailyRegistrationsLast30Days.Select(item => new DashboardDailyRegistrationViewModel(item.DateLabel, item.Count)).ToList(),
            DailyPaidSubscriptionsLast30Days = dto.DailyPaidSubscriptionsLast30Days.Select(item => new DashboardDailyRegistrationViewModel(item.DateLabel, item.Count)).ToList(),
            DailySubscriptionPlanPurchases = dto.DailySubscriptionPlanPurchases.Select(item => new DashboardSubscriptionPlanPurchaseViewModel(item.PlanTitle, item.Count, item.Amount)).ToList(),
            YesterdaySubscriptionPlanPurchases = dto.YesterdaySubscriptionPlanPurchases.Select(item => new DashboardSubscriptionPlanPurchaseViewModel(item.PlanTitle, item.Count, item.Amount)).ToList(),
            MonthlySubscriptionPlanPurchases = dto.MonthlySubscriptionPlanPurchases.Select(item => new DashboardSubscriptionPlanPurchaseViewModel(item.PlanTitle, item.Count, item.Amount)).ToList(),
            RecentUsers = dto.RecentUsers.Select(UserListItemViewModel.FromDto).ToList(),
            RecentPosts = dto.RecentPosts.Select(BlogListItemViewModel.FromDto).ToList(),
            PaymentSummary = new DashboardPaymentSummaryViewModel
            {
                SuccessfulAmount = dto.PaymentSummary.SuccessfulAmount,
                SuccessfulCount = dto.PaymentSummary.SuccessfulCount,
                FailedCount = dto.PaymentSummary.FailedCount,
                PendingCount = dto.PaymentSummary.PendingCount
            },
            RecentPayments = dto.RecentPayments.Select(item => new DashboardPaymentActivityViewModel(
                item.CustomerName,
                item.PlanTitle ?? "اشتراک",
                ToStatusLabel(item.Status),
                item.Amount,
                item.CreatedAtUtc,
                item.PaidAtUtc,
                item.TrackId,
                item.Status)).ToList()
        };
    }

    private static string ToStatusLabel(PaymentStatus status) => status switch
    {
        PaymentStatus.Paid => "موفق",
        PaymentStatus.Pending => "در انتظار",
        PaymentStatus.Redirected => "در انتظار",
        PaymentStatus.Failed => "ناموفق",
        PaymentStatus.Cancelled => "لغو شده",
        _ => status.ToString()
    };
}

public record DashboardPopularPostViewModel(string Title, long VisitCount);

public record DashboardRoleDistributionViewModel(string RoleName, int UserCount);

public record DashboardPermissionUsageViewModel(string PermissionName, int RoleCount);

public record DashboardMonthlyRegistrationViewModel(string Month, int Count);

public record DashboardDailyRegistrationViewModel(string DateLabel, int Count);

public record DashboardSubscriptionPlanPurchaseViewModel(string PlanTitle, int Count, long Amount);

public class DashboardPaymentSummaryViewModel
{
    public long SuccessfulAmount { get; init; }
    public int SuccessfulCount { get; init; }
    public int FailedCount { get; init; }
    public int PendingCount { get; init; }
}

public record DashboardPaymentActivityViewModel(
    string CustomerName,
    string PlanTitle,
    string StatusLabel,
    int Amount,
    DateTime CreatedAtUtc,
    DateTime? PaidAtUtc,
    string? TrackId,
    PaymentStatus Status);
