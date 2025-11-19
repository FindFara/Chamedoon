using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Services.Admin.Common.Models;
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
            .SumAsync(p => (long)p.Amount, cancellationToken);
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
                p.Amount,
                p.Status,
                p.CreatedAtUtc,
                p.PaidAtUtc,
                p.GatewayTrackId))
            .ToList();
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

