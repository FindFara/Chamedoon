using System;
using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminRoleRepository : IAdminRoleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;

    public AdminRoleRepository(ApplicationDbContext context, RoleManager<Role> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public Task<List<Role>> GetRolesAsync(CancellationToken cancellationToken)
        => _roleManager.Roles
            .Include(role => role.RolePermissions)
            .AsNoTracking()
            .OrderBy(role => role.Name)
            .ToListAsync(cancellationToken);

    public Task<Role?> GetRoleAsync(long id, CancellationToken cancellationToken)
        => _roleManager.Roles
            .Include(role => role.RolePermissions)
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.Id == id, cancellationToken);

    public async Task<(IdentityResult Result, Role? Entity)> CreateRoleAsync(Role role, IEnumerable<string> permissionNames, CancellationToken cancellationToken)
    {
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            return (result, role);
        }

        await ReplacePermissionsAsync(role.Id, permissionNames, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return (result, await GetRoleAsync(role.Id, cancellationToken));
    }

    public async Task<IdentityResult> UpdateRoleAsync(Role role, IEnumerable<string> permissionNames, CancellationToken cancellationToken)
    {
        var existing = await _roleManager.FindByIdAsync(role.Id.ToString());
        if (existing is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "نقش یافت نشد." });
        }

        existing.Name = role.Name;
        existing.NormalizedName = role.NormalizedName;

        var result = await _roleManager.UpdateAsync(existing);
        if (!result.Succeeded)
        {
            return result;
        }

        await ReplacePermissionsAsync(existing.Id, permissionNames, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteRoleAsync(long id, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "نقش یافت نشد." });
        }

        var permissions = await _context.RolePermission.Where(permission => permission.RoleId == id).ToListAsync(cancellationToken);
        if (permissions.Any())
        {
            _context.RolePermission.RemoveRange(permissions);
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            return result;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public Task<List<string>> GetDistinctPermissionNamesAsync(CancellationToken cancellationToken)
        => _context.RolePermission
            .Where(permission => !string.IsNullOrWhiteSpace(permission.PermissionName))
            .Select(permission => permission.PermissionName!)
            .Distinct()
            .OrderBy(name => name)
            .ToListAsync(cancellationToken);

    public async Task<Dictionary<string, int>> GetPermissionUsageAsync(CancellationToken cancellationToken)
    {
        return await _context.RolePermission
            .Where(permission => permission.RoleId != null && !string.IsNullOrWhiteSpace(permission.PermissionName))
            .GroupBy(permission => permission.PermissionName!)
            .Select(group => new { group.Key, Count = group.Select(item => item.RoleId).Distinct().Count() })
            .ToDictionaryAsync(item => item.Key, item => item.Count, cancellationToken);
    }

    private async Task ReplacePermissionsAsync(long roleId, IEnumerable<string> permissionNames, CancellationToken cancellationToken)
    {
        var existing = await _context.RolePermission.Where(permission => permission.RoleId == roleId).ToListAsync(cancellationToken);
        if (existing.Any())
        {
            _context.RolePermission.RemoveRange(existing);
        }

        var newPermissions = permissionNames
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(name => new RolePermission
            {
                RoleId = roleId,
                PermissionName = name,
                PermissionTitle = name
            })
            .ToList();

        if (newPermissions.Count > 0)
        {
            await _context.RolePermission.AddRangeAsync(newPermissions, cancellationToken);
        }
    }
}
