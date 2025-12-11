using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Services.Subscription;

namespace Chamedoon.Application.Services.Admin.Dashboard;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminUserRepository _userRepository;
    private readonly IAdminBlogRepository _blogRepository;
    private readonly IAdminRoleRepository _roleRepository;
    private readonly IAdminPaymentRepository _paymentRepository;

    public AdminDashboardService(
        IAdminUserRepository userRepository,
        IAdminBlogRepository blogRepository,
        IAdminRoleRepository roleRepository,
        IAdminPaymentRepository paymentRepository)
    {
        _userRepository = userRepository;
        _blogRepository = blogRepository;
        _roleRepository = roleRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<OperationResult<DashboardSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1);

        var totalUsers = await _userRepository.CountUsersAsync(cancellationToken);
        var activeUsers = await _userRepository.CountActiveUsersAsync(cancellationToken);
        var usersWithActiveSubscription = await _userRepository.CountActiveSubscriptionsAsync(cancellationToken);
        var newUsers = await _userRepository.CountUsersCreatedSinceAsync(monthStart, cancellationToken);
        var totalPosts = await _blogRepository.CountArticlesAsync(cancellationToken);
        var publishedPosts = await _blogRepository.CountPublishedArticlesAsync(cancellationToken);
        var totalViews = await _blogRepository.SumArticleViewsAsync(cancellationToken);
        var popularPosts = await _blogRepository.GetTopArticlesAsync(5, cancellationToken);
        var roleCounts = await _userRepository.GetRoleUserCountsAsync(cancellationToken);
        var roles = await _roleRepository.GetRolesAsync(cancellationToken);
        var permissionUsage = await _roleRepository.GetPermissionUsageAsync(cancellationToken);
        var recentUsers = await _userRepository.GetRecentUsersAsync(5, cancellationToken);
        var recentPosts = await _blogRepository.GetRecentArticlesAsync(5, cancellationToken);
        const int monthlyWindow = 6;
        var paymentSummarySince = now.AddDays(-30);
        var monthlyRegistrations = await _userRepository.GetMonthlyRegistrationCountsAsync(monthlyWindow, cancellationToken);
        var monthlySubscriptions = await _userRepository.GetMonthlyActiveSubscriptionCountsAsync(monthlyWindow, cancellationToken);
        var monthlyBlogViews = await _blogRepository.GetMonthlyArticleViewCountsAsync(monthlyWindow, cancellationToken);
        var paymentSummary = await _paymentRepository.GetPaymentSummaryAsync(paymentSummarySince, cancellationToken);
        var paymentActivities = await _paymentRepository.GetRecentPaymentsAsync(5, cancellationToken);
        var mappedPayments = paymentActivities
            .Select(activity =>
            {
                var plan = SubscriptionPlanCatalog.Find(activity.PlanId);
                return activity with { PlanTitle = plan?.Title ?? activity.PlanTitle ?? "اشتراک" };
            })
            .ToList();

        var summary = new DashboardSummaryDto
        {
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            NewUsersThisMonth = newUsers,
            TotalBlogPosts = totalPosts,
            PublishedBlogPosts = publishedPosts,
            DraftBlogPosts = Math.Max(totalPosts - publishedPosts, 0),
            TotalViews = totalViews,
            UsersWithActiveSubscription = usersWithActiveSubscription,
            PopularPosts = popularPosts.Select(article => new DashboardPopularPostDto(article.ArticleTitle, article.VisitCount)).ToList(),
            RoleDistribution = BuildRoleDistribution(roleCounts, roles),
            PermissionUsage = permissionUsage
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                .Select(kvp => new DashboardPermissionUsageDto(kvp.Key, kvp.Value))
                .OrderByDescending(item => item.RoleCount)
                .ToList(),
            MonthlyRegistrations = BuildMonthlyRegistrations(monthlyRegistrations),
            MonthlyActiveSubscriptions = BuildMonthlyRegistrations(monthlySubscriptions),
            MonthlyBlogViews = BuildMonthlyRegistrations(monthlyBlogViews),
            RecentUsers = recentUsers.Select(user => user.ToAdminUserDto()).ToList(),
            RecentPosts = recentPosts.Select(article => article.ToAdminBlogPostDto()).ToList(),
            PaymentSummary = paymentSummary,
            RecentPayments = mappedPayments
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
