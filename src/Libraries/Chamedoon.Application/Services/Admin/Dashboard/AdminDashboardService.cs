using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Dashboard;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminUserRepository _userRepository;
    private readonly IAdminBlogRepository _blogRepository;
    private readonly IAdminRoleRepository _roleRepository;

    public AdminDashboardService(
        IAdminUserRepository userRepository,
        IAdminBlogRepository blogRepository,
        IAdminRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _blogRepository = blogRepository;
        _roleRepository = roleRepository;
    }

    public async Task<OperationResult<DashboardSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1);

        var totalUsersTask = _userRepository.CountUsersAsync(cancellationToken);
        var activeUsersTask = _userRepository.CountActiveUsersAsync(cancellationToken);
        var newUsersTask = _userRepository.CountUsersCreatedSinceAsync(monthStart, cancellationToken);
        var totalPostsTask = _blogRepository.CountArticlesAsync(cancellationToken);
        var publishedPostsTask = _blogRepository.CountPublishedArticlesAsync(cancellationToken);
        var totalViewsTask = _blogRepository.SumArticleViewsAsync(cancellationToken);
        var popularPostsTask = _blogRepository.GetTopArticlesAsync(5, cancellationToken);
        var roleCountsTask = _userRepository.GetRoleUserCountsAsync(cancellationToken);
        var rolesTask = _roleRepository.GetRolesAsync(cancellationToken);
        var permissionUsageTask = _roleRepository.GetPermissionUsageAsync(cancellationToken);
        var recentUsersTask = _userRepository.GetRecentUsersAsync(5, cancellationToken);
        var recentPostsTask = _blogRepository.GetRecentArticlesAsync(5, cancellationToken);
        var monthlyRegistrationsTask = _userRepository.GetMonthlyRegistrationCountsAsync(6, cancellationToken);

        await Task.WhenAll(
            totalUsersTask,
            activeUsersTask,
            newUsersTask,
            totalPostsTask,
            publishedPostsTask,
            totalViewsTask,
            popularPostsTask,
            roleCountsTask,
            rolesTask,
            permissionUsageTask,
            recentUsersTask,
            recentPostsTask,
            monthlyRegistrationsTask);

        var summary = new DashboardSummaryDto
        {
            TotalUsers = totalUsersTask.Result,
            ActiveUsers = activeUsersTask.Result,
            NewUsersThisMonth = newUsersTask.Result,
            TotalBlogPosts = totalPostsTask.Result,
            PublishedBlogPosts = publishedPostsTask.Result,
            DraftBlogPosts = Math.Max(totalPostsTask.Result - publishedPostsTask.Result, 0),
            TotalViews = totalViewsTask.Result,
            PopularPosts = popularPostsTask.Result.Select(article => new DashboardPopularPostDto(article.ArticleTitle, article.VisitCount)).ToList(),
            RoleDistribution = BuildRoleDistribution(roleCountsTask.Result, rolesTask.Result),
            PermissionUsage = permissionUsageTask.Result
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                .Select(kvp => new DashboardPermissionUsageDto(kvp.Key, kvp.Value))
                .OrderByDescending(item => item.RoleCount)
                .ToList(),
            MonthlyRegistrations = BuildMonthlyRegistrations(monthlyRegistrationsTask.Result),
            RecentUsers = recentUsersTask.Result.Select(user => user.ToAdminUserDto()).ToList(),
            RecentPosts = recentPostsTask.Result.Select(article => article.ToAdminBlogPostDto()).ToList()
        };

        return OperationResult<DashboardSummaryDto>.Success(summary);
    }

    private static IReadOnlyList<DashboardRoleDistributionDto> BuildRoleDistribution(Dictionary<long, int> counts, List<Domin.Entity.Permissions.Role> roles)
    {
        var lookup = roles.ToDictionary(role => role.Id, role => role.Name ?? "بدون نام");
        return counts
            .Select(kvp => new DashboardRoleDistributionDto(lookup.TryGetValue(kvp.Key, out var name) ? name : "نامشخص", kvp.Value))
            .OrderByDescending(item => item.UserCount)
            .ToList();
    }

    private static IReadOnlyList<DashboardMonthlyRegistrationDto> BuildMonthlyRegistrations(IReadOnlyList<MonthlyRegistrationCount> counts)
    {
        return counts
            .OrderBy(record => new DateTime(record.Year, record.Month, 1))
            .Select(record =>
            {
                var date = new DateTime(record.Year, record.Month, 1);
                return new DashboardMonthlyRegistrationDto(date.ToString("MMMM yyyy", new CultureInfo("fa-IR")), record.Count);
            })
            .ToList();
    }
}
