using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;

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
    public IReadOnlyList<DashboardPopularPostViewModel> PopularPosts { get; init; } = Array.Empty<DashboardPopularPostViewModel>();
    public IReadOnlyList<DashboardRoleDistributionViewModel> RoleDistribution { get; init; } = Array.Empty<DashboardRoleDistributionViewModel>();
    public IReadOnlyList<DashboardPermissionUsageViewModel> PermissionUsage { get; init; } = Array.Empty<DashboardPermissionUsageViewModel>();
    public IReadOnlyList<DashboardMonthlyRegistrationViewModel> MonthlyRegistrations { get; init; } = Array.Empty<DashboardMonthlyRegistrationViewModel>();
    public IReadOnlyList<UserListItemViewModel> RecentUsers { get; init; } = Array.Empty<UserListItemViewModel>();
    public IReadOnlyList<BlogListItemViewModel> RecentPosts { get; init; } = Array.Empty<BlogListItemViewModel>();

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
            PopularPosts = dto.PopularPosts.Select(item => new DashboardPopularPostViewModel(item.Title, item.VisitCount)).ToList(),
            RoleDistribution = dto.RoleDistribution.Select(item => new DashboardRoleDistributionViewModel(item.RoleName, item.UserCount)).ToList(),
            PermissionUsage = dto.PermissionUsage.Select(item => new DashboardPermissionUsageViewModel(item.PermissionName, item.RoleCount)).ToList(),
            MonthlyRegistrations = dto.MonthlyRegistrations.Select(item => new DashboardMonthlyRegistrationViewModel(item.Month, item.Count)).ToList(),
            RecentUsers = dto.RecentUsers.Select(UserListItemViewModel.FromDto).ToList(),
            RecentPosts = dto.RecentPosts.Select(BlogListItemViewModel.FromDto).ToList()
        };
    }
}

public record DashboardPopularPostViewModel(string Title, long VisitCount);

public record DashboardRoleDistributionViewModel(string RoleName, int UserCount);

public record DashboardPermissionUsageViewModel(string PermissionName, int RoleCount);

public record DashboardMonthlyRegistrationViewModel(string Month, int Count);
