using System;
using System.Collections.Generic;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Users;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminUserRepository
{
    Task<List<User>> GetUsersAsync(string? search, long? roleId, CancellationToken cancellationToken);
    Task<User?> GetUserAsync(long id, CancellationToken cancellationToken);
    Task<(IdentityResult Result, User? Entity)> CreateUserAsync(User user, string password, long? roleId, Customer? customer, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateUserAsync(User user, long? roleId, string? password, Customer? customer, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteUserAsync(long id, CancellationToken cancellationToken);
    Task<List<Role>> GetRolesAsync(CancellationToken cancellationToken);
    Task<int> CountUsersAsync(CancellationToken cancellationToken);
    Task<int> CountActiveUsersAsync(CancellationToken cancellationToken);
    Task<int> CountActiveSubscriptionsAsync(CancellationToken cancellationToken);
    Task<int> CountUsersCreatedSinceAsync(DateTime since, CancellationToken cancellationToken);
    Task<Dictionary<long, int>> GetRoleUserCountsAsync(CancellationToken cancellationToken);
    Task<List<User>> GetRecentUsersAsync(int count, CancellationToken cancellationToken);
    Task<IReadOnlyList<MonthlyRegistrationCount>> GetMonthlyRegistrationCountsAsync(int months, CancellationToken cancellationToken);
    Task<IReadOnlyList<MonthlyRegistrationCount>> GetMonthlyActiveSubscriptionCountsAsync(int months, CancellationToken cancellationToken);
}
