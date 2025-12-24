using System;
using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminUserRepository : IAdminUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Domin.Entity.Permissions.Role> _roleManager;

    public AdminUserRepository(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<Domin.Entity.Permissions.Role> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<PaginatedList<User>> GetUsersAsync(string? search, long? roleId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        IQueryable<User> query = _userManager.Users
            .Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role)
            .Include(u => u.Customer);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(u =>
                (u.Email ?? string.Empty).Contains(term) ||
                (u.UserName ?? string.Empty).Contains(term) ||
                (u.Customer != null &&
                 ((u.Customer.FirstName ?? string.Empty) + " " + (u.Customer.LastName ?? string.Empty))
                    .Contains(term)));
        }

        if (roleId.HasValue)
        {
            query = query.Where(u => u.UserRoles!.Any(ur => ur.RoleId == roleId.Value));
        }

        var ordered = query
            .AsNoTracking()
            .OrderByDescending(u => u.Created);

        return await PaginatedList<User>.CreateAsync(ordered, pageNumber, pageSize);
    }

    public Task<User?> GetUserAsync(long id, CancellationToken cancellationToken)
        => _userManager.Users
            .Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role)
            .Include(u => u.Customer)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<(IdentityResult Result, User? Entity)> CreateUserAsync(User user, string password, long? roleId, Customer? customer, CancellationToken cancellationToken)
    {
        user.SecurityStamp ??= Guid.NewGuid().ToString();
        user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
        user.NormalizedUserName = _userManager.NormalizeName(user.UserName);

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (result, user);
        }

        await UpdateCustomerAsync(user, customer, cancellationToken);
        await UpdateRolesAsync(user, roleId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return (result, user);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user, long? roleId, string? password, Customer? customer, CancellationToken cancellationToken)
    {
        var existing = await _userManager.Users
            .Include(u => u.Customer)
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);

        if (existing is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "کاربر یافت نشد." });
        }

        existing.Email = user.Email;
        existing.UserName = user.UserName;
        existing.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
        existing.NormalizedUserName = _userManager.NormalizeName(user.UserName);
        existing.PhoneNumber = user.PhoneNumber;
        existing.PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(user.PhoneNumber);
        existing.LockoutEnd = user.LockoutEnd;
        existing.LockoutEnabled = true;
        existing.LastModified = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(existing);
        if (!updateResult.Succeeded)
        {
            return updateResult;
        }

        if (!string.IsNullOrWhiteSpace(password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(existing);
            var passwordResult = await _userManager.ResetPasswordAsync(existing, token, password);
            if (!passwordResult.Succeeded)
            {
                return passwordResult;
            }
        }

        await UpdateCustomerAsync(existing, customer, cancellationToken);
        await UpdateRolesAsync(existing, roleId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteUserAsync(long id, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "کاربر یافت نشد." });
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return result;
        }

        var customer = await _context.Customers
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.User.Id == id, cancellationToken);

        if (customer is not null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return IdentityResult.Success;
    }

    public Task<List<Domin.Entity.Permissions.Role>> GetRolesAsync(CancellationToken cancellationToken)
        => _roleManager.Roles
            .Include(r => r.RolePermissions)
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

    public Task<int> CountUsersAsync(CancellationToken cancellationToken)
        => _userManager.Users.CountAsync(cancellationToken);

    public Task<int> CountActiveUsersAsync(CancellationToken cancellationToken)
        => _userManager.Users.CountAsync(u => !u.LockoutEnd.HasValue || u.LockoutEnd <= DateTimeOffset.UtcNow, cancellationToken);

    public Task<int> CountUsersCreatedSinceAsync(DateTime since, CancellationToken cancellationToken)
        => _userManager.Users.CountAsync(u => u.Created >= since, cancellationToken);

    public Task<int> CountActiveSubscriptionsAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        return _context.Customers.CountAsync(customer =>
            !string.IsNullOrWhiteSpace(customer.SubscriptionPlanId) &&
            customer.SubscriptionEndDateUtc.HasValue &&
            customer.SubscriptionEndDateUtc.Value > now,
            cancellationToken);
    }

    public async Task<Dictionary<long, int>> GetRoleUserCountsAsync(CancellationToken cancellationToken)
    {
        return await _context.UserRole
            .GroupBy(ur => ur.RoleId)
            .Select(group => new { group.Key, Count = group.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count, cancellationToken);
    }

    public Task<List<User>> GetRecentUsersAsync(int count, CancellationToken cancellationToken)
        => _userManager.Users
            .IgnoreQueryFilters()
            .Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role)
            .Include(u => u.Customer)
            .OrderByDescending(u => u.Created)
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<MonthlyRegistrationCount>> GetMonthlyRegistrationCountsAsync(int months, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var start = new DateTime(now.Year, now.Month, 1).AddMonths(-(months - 1));

        var data = await _userManager.Users
            .Where(u => u.Created >= start)
            .GroupBy(u => new { u.Created.Year, u.Created.Month })
            .Select(g => new MonthlyRegistrationCount(g.Key.Year, g.Key.Month, g.Count()))
            .ToListAsync(cancellationToken);

        var results = new List<MonthlyRegistrationCount>();
        for (var i = 0; i < months; i++)
        {
            var date = start.AddMonths(i);
            var match = data.FirstOrDefault(record => record.Year == date.Year && record.Month == date.Month);
            results.Add(match ?? new MonthlyRegistrationCount(date.Year, date.Month, 0));
        }

        return results;
    }

    public async Task<IReadOnlyList<MonthlyRegistrationCount>> GetMonthlyActiveSubscriptionCountsAsync(int months, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var start = new DateTime(now.Year, now.Month, 1).AddMonths(-(months - 1));
        var end = start.AddMonths(months);

        var candidates = await _context.Customers
            .Where(customer =>
                !string.IsNullOrWhiteSpace(customer.SubscriptionPlanId) &&
                customer.SubscriptionEndDateUtc.HasValue &&
                customer.SubscriptionEndDateUtc.Value >= start)
            .Select(customer => new
            {
                Start = customer.SubscriptionStartDateUtc ?? DateTime.MinValue,
                End = customer.SubscriptionEndDateUtc!.Value
            })
            .ToListAsync(cancellationToken);

        var results = new List<MonthlyRegistrationCount>();

        for (var i = 0; i < months; i++)
        {
            var monthStart = start.AddMonths(i);
            var monthEnd = monthStart.AddMonths(1);
            if (monthStart >= end)
            {
                break;
            }

            var count = candidates.Count(window => window.Start < monthEnd && window.End >= monthStart);
            results.Add(new MonthlyRegistrationCount(monthStart.Year, monthStart.Month, count));
        }

        return results;
    }

    public async Task<IReadOnlyList<DailyRegistrationCount>> GetDailyRegistrationCountsAsync(int days, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var start = today.AddDays(-(days - 1));

        var rawData = await _userManager.Users
            .Where(user => user.Created.Date >= start)
            .GroupBy(user => user.Created.Date)
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

    private async Task UpdateCustomerAsync(User user, Customer? customer, CancellationToken cancellationToken)
    {
        var existing = await _context.Customers
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.User.Id == user.Id, cancellationToken);

        if (customer is null)
        {
            if (existing is not null)
            {
                _context.Customers.Remove(existing);
            }

            return;
        }

        if (existing is null)
        {
            customer.User = user;
            await _context.Customers.AddAsync(customer, cancellationToken);
        }
        else
        {
            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Description = customer.Description;
            existing.Job = customer.Job;
            existing.SubscriptionPlanId = customer.SubscriptionPlanId;
            existing.SubscriptionStartDateUtc = customer.SubscriptionStartDateUtc;
            existing.SubscriptionEndDateUtc = customer.SubscriptionEndDateUtc;
            existing.UsedEvaluations = customer.UsedEvaluations;
        }
    }

    private async Task UpdateRolesAsync(User user, long? roleId, CancellationToken cancellationToken)
    {
        var currentRoles = await _userManager.GetRolesAsync(user);

        if (roleId.HasValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId.Value.ToString());
            if (role is not null && !string.IsNullOrWhiteSpace(role.Name))
            {
                var targetRole = role.Name;
                var rolesToRemove = currentRoles
                    .Where(r => !string.Equals(r, targetRole, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (rolesToRemove.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                }

                if (!currentRoles.Any(r => string.Equals(r, targetRole, StringComparison.OrdinalIgnoreCase)))
                {
                    await _userManager.AddToRoleAsync(user, targetRole);
                }
            }
        }
        else if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }
    }
}
