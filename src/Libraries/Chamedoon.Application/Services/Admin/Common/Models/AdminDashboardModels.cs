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
    public IReadOnlyList<AdminUserDto> RecentUsers { get; set; } = Array.Empty<AdminUserDto>();
    public IReadOnlyList<AdminBlogPostDto> RecentPosts { get; set; } = Array.Empty<AdminBlogPostDto>();
}

public record DashboardPopularPostDto(string Title, long VisitCount);

public record DashboardRoleDistributionDto(string RoleName, int UserCount);

public record DashboardPermissionUsageDto(string PermissionName, int RoleCount);

public record DashboardMonthlyRegistrationDto(string Month, int Count);
