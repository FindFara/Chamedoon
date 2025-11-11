using System.Collections.Generic;
using Chamedoon.Domin.Entity.Permissions;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminRoleRepository
{
    Task<List<Role>> GetRolesAsync(CancellationToken cancellationToken);
    Task<Role?> GetRoleAsync(long id, CancellationToken cancellationToken);
    Task<(IdentityResult Result, Role? Entity)> CreateRoleAsync(Role role, IEnumerable<string> permissionNames, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateRoleAsync(Role role, IEnumerable<string> permissionNames, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteRoleAsync(long id, CancellationToken cancellationToken);
    Task<List<string>> GetDistinctPermissionNamesAsync(CancellationToken cancellationToken);
    Task<Dictionary<string, int>> GetPermissionUsageAsync(CancellationToken cancellationToken);
}
