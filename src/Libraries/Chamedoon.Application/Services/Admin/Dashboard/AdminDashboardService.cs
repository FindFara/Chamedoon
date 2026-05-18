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
    private readonly SubscriptionService _subscriptionService;

    public AdminDashboardService(
        IAdminUserRepository userRepository,
        IAdminBlogRepository blogRepository,
        IAdminRoleRepository roleRepository,
        IAdminPaymentRepository paymentRepository,
        SubscriptionService subscriptionService)
    {
        _userRepository = userRepository;
        _blogRepository = blogRepository;
        _roleRepository = roleRepository;
        _paymentRepository = paymentRepository;
        _subscriptionService = subscriptionService;
    }

    public async Task<OperationResult<DashboardSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1);
        var dayStart = now.Date;
        var dayEnd = dayStart.AddDays(1);
        var yesterdayStart = dayStart.AddDays(-1);

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
        var dailyRegistrations = await _userRepository.GetDailyRegistrationCountsAsync(30, cancellationToken);
        var dailyPaidSubscriptions = await _paymentRepository.GetDailyPaidSubscriptionCountsAsync(30, cancellationToken);
        var paymentSummary = await _paymentRepository.GetPaymentSummaryAsync(paymentSummarySince, cancellationToken);
        var paymentActivities = await _paymentRepository.GetRecentPaymentsAsync(5, cancellationToken);
        var dailyPlanPurchases = await _paymentRepository.GetSubscriptionPlanPurchasesAsync(dayStart, dayEnd, cancellationToken);
        var yesterdayPlanPurchases = await _paymentRepository.GetSubscriptionPlanPurchasesAsync(yesterdayStart, dayStart, cancellationToken);
        var monthlyPlanPurchases = await _paymentRepository.GetSubscriptionPlanPurchasesAsync(monthStart, monthEnd, cancellationToken);
        var planTitles = await _subscriptionService.GetPlanTitleLookupAsync(cancellationToken);
        var mappedPayments = paymentActivities
            .Select(activity =>
            {
                var planTitle = !string.IsNullOrWhiteSpace(activity.PlanId) && planTitles.TryGetValue(activity.PlanId, out var title)
                    ? title
                    : activity.PlanTitle ?? "اشتراک";
                return activity with { PlanTitle = planTitle };
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
            DailyRegistrationsLast30Days = BuildDailyRegistrations(dailyRegistrations),
            DailyPaidSubscriptionsLast30Days = BuildDailyRegistrations(dailyPaidSubscriptions),
            DailySubscriptionPlanPurchases = dailyPlanPurchases,
            YesterdaySubscriptionPlanPurchases = yesterdayPlanPurchases,
            MonthlySubscriptionPlanPurchases = monthlyPlanPurchases,
            RecentUsers = recentUsers.Select(user => user.ToAdminUserDto(planTitles)).ToList(),
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

    private static IReadOnlyList<DashboardDailyRegistrationDto> BuildDailyRegistrations(IReadOnlyList<DailyRegistrationCount> counts)
    {
        return counts
            .OrderBy(record => record.Date)
            .Select(record => new DashboardDailyRegistrationDto(
                record.Date.ToString("dd MMM", new CultureInfo("fa-IR")),
                record.Count))
            .ToList();
    }
}
