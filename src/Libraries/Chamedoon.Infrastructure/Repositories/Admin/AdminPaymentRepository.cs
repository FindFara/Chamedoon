using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Enums;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminPaymentRepository : IAdminPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public AdminPaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardPaymentSummaryDto> GetPaymentSummaryAsync(DateTime since, CancellationToken cancellationToken)
    {
        var query = _context.PaymentRequests.AsNoTracking().Where(p => p.CreatedAtUtc >= since);

        var successfulAmount = await query.Where(p => p.Status == PaymentStatus.Paid)
            .SumAsync(p => (long)(p.FinalAmount > 0 ? p.FinalAmount : p.Amount), cancellationToken);
        var successfulCount = await query.CountAsync(p => p.Status == PaymentStatus.Paid, cancellationToken);
        var failedCount = await query.CountAsync(p => p.Status == PaymentStatus.Failed, cancellationToken);
        var pendingCount = await query.CountAsync(p => p.Status == PaymentStatus.Pending || p.Status == PaymentStatus.Redirected, cancellationToken);

        return new DashboardPaymentSummaryDto
        {
            SuccessfulAmount = successfulAmount,
            SuccessfulCount = successfulCount,
            FailedCount = failedCount,
            PendingCount = pendingCount
        };
    }

    public async Task<IReadOnlyList<DashboardPaymentActivityDto>> GetRecentPaymentsAsync(int count, CancellationToken cancellationToken)
    {
        var payments = await _context.PaymentRequests
            .Include(p => p.Customer)
                .ThenInclude(c => c.User)
            .OrderByDescending(p => p.CreatedAtUtc)
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return payments
            .Select(p => new DashboardPaymentActivityDto(
                p.Id,
                p.PlanId,
                null,
                BuildCustomerName(p.Customer),
                p.Customer?.User?.Email,
                p.FinalAmount > 0 ? p.FinalAmount : p.Amount,
                p.Status,
                p.CreatedAtUtc,
                p.PaidAtUtc,
                p.GatewayTrackId))
            .ToList();
    }

    public async Task<IReadOnlyList<DailyRegistrationCount>> GetDailyPaidSubscriptionCountsAsync(int days, CancellationToken cancellationToken)
    {
        var today = DateTime.Now.Date;
        var start = today.AddDays(-(days - 1));

        var rawData = await _context.PaymentRequests
            .AsNoTracking()
            .Where(p => p.Status == PaymentStatus.Paid &&
                        p.PaidAtUtc.HasValue &&
                        p.PaidAtUtc.Value.Date >= start)
            .GroupBy(p => p.PaidAtUtc!.Value.Date)
            .Select(group => new DailyRegistrationCount(group.Key, group.Count()))
            .ToListAsync(cancellationToken);

        var results = new List<DailyRegistrationCount>();
        for (var i = 0; i < days; i++)
        {
            var date = start.AddDays(i);
            var match = rawData.FirstOrDefault(record => record.Date.Date == date);
            results.Add(match ?? new DailyRegistrationCount(date, 0));
        }

        return results;
    }

    public async Task<IReadOnlyList<DashboardSubscriptionPlanPurchaseDto>> GetSubscriptionPlanPurchasesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken)
    {
        var planCounts = await _context.PaymentRequests
            .AsNoTracking()
            .Where(p => p.Status == PaymentStatus.Paid &&
                        p.PaidAtUtc.HasValue &&
                        p.PaidAtUtc.Value >= fromUtc &&
                        p.PaidAtUtc.Value < toUtc)
            .GroupBy(p => p.PlanId)
            .Select(group => new { PlanId = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        var planTitles = await _context.SubscriptionPlans
            .AsNoTracking()
            .ToDictionaryAsync(plan => plan.Id, plan => plan.Title, cancellationToken);

        return planCounts
            .Select(item =>
            {
                var title = item.PlanId != null && planTitles.TryGetValue(item.PlanId, out var planTitle)
                    ? planTitle
                    : "نامشخص";
                return new DashboardSubscriptionPlanPurchaseDto(title, item.Count);
            })
            .OrderByDescending(item => item.Count)
            .ToList();
    }

    public Task<PaginatedList<PaymentRequest>> GetPaymentsAsync(
        string? search,
        PaymentStatus? status,
        DateTime? fromDate,
        DateTime? toDate,
        string? userName,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = _context.PaymentRequests
            .Include(p => p.Customer)
                .ThenInclude(c => c.User)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.Trim().ToLower();
            query = query.Where(p =>
                (!string.IsNullOrEmpty(p.GatewayTrackId) && p.GatewayTrackId.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(p.ReferenceCode) && p.ReferenceCode.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(p.PlanId) && p.PlanId.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(lowered)) ||
                (p.Customer != null && (
                    (!string.IsNullOrEmpty(p.Customer.FirstName) && p.Customer.FirstName.ToLower().Contains(lowered)) ||
                    (!string.IsNullOrEmpty(p.Customer.LastName) && p.Customer.LastName.ToLower().Contains(lowered)) ||
                    (p.Customer.User != null && (
                        (!string.IsNullOrEmpty(p.Customer.User.UserName) && p.Customer.User.UserName.ToLower().Contains(lowered)) ||
                        (!string.IsNullOrEmpty(p.Customer.User.Email) && p.Customer.User.Email.ToLower().Contains(lowered))
                    ))
                )));
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        if (fromDate.HasValue)
        {
            var fromLocal = DateTime.SpecifyKind(fromDate.Value.Date, DateTimeKind.Local);
            query = query.Where(p => p.CreatedAtUtc >= fromLocal);
        }

        if (toDate.HasValue)
        {
            var toExclusive = DateTime.SpecifyKind(toDate.Value.Date.AddDays(1), DateTimeKind.Local);
            query = query.Where(p => p.CreatedAtUtc < toExclusive);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var lowered = userName.Trim().ToLower();
            query = query.Where(p => p.Customer != null && p.Customer.User != null && (
                (!string.IsNullOrEmpty(p.Customer.User.UserName) && p.Customer.User.UserName.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(p.Customer.FirstName) && p.Customer.FirstName.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(p.Customer.LastName) && p.Customer.LastName.ToLower().Contains(lowered))));
        }

        query = query.OrderByDescending(p => p.CreatedAtUtc);

        return PaginatedList<PaymentRequest>.CreateAsync(query, pageNumber, pageSize);
    }

    private static string BuildCustomerName(Domin.Entity.Customers.Customer? customer)
    {
        if (customer?.User is null)
        {
            return "کاربر";
        }

        var fullName = string.Join(" ", new[] { customer.FirstName, customer.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            return fullName;
        }

        return string.IsNullOrWhiteSpace(customer.User.UserName) ? customer.User.Email ?? "کاربر" : customer.User.UserName!;
    }
}
