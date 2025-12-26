namespace Chamedoon.Application.Services.Admin.Common.Models;

public class DashboardSummaryDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int TotalBlogPosts { get; set; }
    public int PublishedBlogPosts { get; set; }
    public int DraftBlogPosts { get; set; }
    public long TotalViews { get; set; }
    public int UsersWithActiveSubscription { get; set; }
    public IReadOnlyList<DashboardPopularPostDto> PopularPosts { get; set; } = Array.Empty<DashboardPopularPostDto>();
    public IReadOnlyList<DashboardRoleDistributionDto> RoleDistribution { get; set; } = Array.Empty<DashboardRoleDistributionDto>();
    public IReadOnlyList<DashboardPermissionUsageDto> PermissionUsage { get; set; } = Array.Empty<DashboardPermissionUsageDto>();
    public IReadOnlyList<DashboardMonthlyRegistrationDto> MonthlyRegistrations { get; set; } = Array.Empty<DashboardMonthlyRegistrationDto>();
    public IReadOnlyList<DashboardMonthlyRegistrationDto> MonthlyActiveSubscriptions { get; set; } = Array.Empty<DashboardMonthlyRegistrationDto>();
    public IReadOnlyList<DashboardMonthlyRegistrationDto> MonthlyBlogViews { get; set; } = Array.Empty<DashboardMonthlyRegistrationDto>();
    public IReadOnlyList<DashboardDailyRegistrationDto> DailyRegistrationsLast30Days { get; set; } = Array.Empty<DashboardDailyRegistrationDto>();
    public IReadOnlyList<DashboardDailyRegistrationDto> DailyPaidSubscriptionsLast30Days { get; set; } = Array.Empty<DashboardDailyRegistrationDto>();
    public IReadOnlyList<AdminUserDto> RecentUsers { get; set; } = Array.Empty<AdminUserDto>();
    public IReadOnlyList<AdminBlogPostDto> RecentPosts { get; set; } = Array.Empty<AdminBlogPostDto>();
    public DashboardPaymentSummaryDto PaymentSummary { get; set; } = new();
    public IReadOnlyList<DashboardPaymentActivityDto> RecentPayments { get; set; } = Array.Empty<DashboardPaymentActivityDto>();
}

public record DashboardPopularPostDto(string Title, long VisitCount);

public record DashboardRoleDistributionDto(string RoleName, int UserCount);

public record DashboardPermissionUsageDto(string PermissionName, int RoleCount);

public record DashboardMonthlyRegistrationDto(string Month, int Count);

public record DashboardDailyRegistrationDto(string DateLabel, int Count);

public class DashboardPaymentSummaryDto
{
    public long SuccessfulAmount { get; set; }
    public int SuccessfulCount { get; set; }
    public int FailedCount { get; set; }
    public int PendingCount { get; set; }
}

public record DashboardPaymentActivityDto(
    long Id,
    string? PlanId,
    string? PlanTitle,
    string CustomerName,
    string? CustomerEmail,
    int Amount,
    Chamedoon.Domin.Enums.PaymentStatus Status,
    DateTime CreatedAtUtc,
    DateTime? PaidAtUtc,
    string? TrackId);
