using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Permissions;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Admin.Roles;

public class AdminRoleService : IAdminRoleService
{
    private readonly IAdminRoleRepository _roleRepository;

    public AdminRoleService(IAdminRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<OperationResult<IReadOnlyList<AdminRoleDto>>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetRolesAsync(cancellationToken);
        var mapped = roles.Select(role => role.ToAdminRoleDto()).ToList();
        return OperationResult<IReadOnlyList<AdminRoleDto>>.Success(mapped);
    }

    public async Task<OperationResult<AdminRoleDto>> GetRoleAsync(long id, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleAsync(id, cancellationToken);
        if (role is null)
        {
            return OperationResult<AdminRoleDto>.Fail("نقش مورد نظر یافت نشد.");
        }

        return OperationResult<AdminRoleDto>.Success(role.ToAdminRoleDto());
    }

    public async Task<OperationResult<AdminRoleDto>> CreateRoleAsync(AdminRoleInput input, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = input.Name,
            NormalizedName = input.Name.ToUpperInvariant()
        };

        var permissions = NormalizePermissions(input.PermissionNames);
        var result = await _roleRepository.CreateRoleAsync(role, permissions, cancellationToken);
        if (!result.Result.Succeeded)
        {
            return OperationResult<AdminRoleDto>.Fail(BuildIdentityErrors(result.Result));
        }

        return OperationResult<AdminRoleDto>.Success(result.Entity!.ToAdminRoleDto());
    }

    public async Task<OperationResult<AdminRoleDto>> UpdateRoleAsync(AdminRoleInput input, CancellationToken cancellationToken)
    {
        if (!input.Id.HasValue)
        {
            return OperationResult<AdminRoleDto>.Fail("شناسه نقش ارسال نشده است.");
        }

        var role = new Role
        {
            Id = input.Id.Value,
            Name = input.Name,
            NormalizedName = input.Name.ToUpperInvariant()
        };

        var permissions = NormalizePermissions(input.PermissionNames);
        var result = await _roleRepository.UpdateRoleAsync(role, permissions, cancellationToken);
        if (!result.Succeeded)
        {
            return OperationResult<AdminRoleDto>.Fail(BuildIdentityErrors(result));
        }

        var updatedRole = await _roleRepository.GetRoleAsync(input.Id.Value, cancellationToken);
        if (updatedRole is null)
        {
            return OperationResult<AdminRoleDto>.Fail("نقش مورد نظر یافت نشد.");
        }

        return OperationResult<AdminRoleDto>.Success(updatedRole.ToAdminRoleDto());
    }

    public async Task<OperationResult<bool>> DeleteRoleAsync(long id, CancellationToken cancellationToken)
    {
        var result = await _roleRepository.DeleteRoleAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            return OperationResult<bool>.Fail(BuildIdentityErrors(result));
        }

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<IReadOnlyList<string>>> GetPermissionNamesAsync(CancellationToken cancellationToken)
    {
        var permissions = await _roleRepository.GetDistinctPermissionNamesAsync(cancellationToken);
        permissions.Sort(StringComparer.OrdinalIgnoreCase);
        return OperationResult<IReadOnlyList<string>>.Success(permissions);
    }

    private static IEnumerable<string> NormalizePermissions(IEnumerable<string> permissions)
        => permissions
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

    private static string BuildIdentityErrors(IdentityResult result)
    {
        if (result.Errors is null)
        {
            return "عملیات با خطا مواجه شد.";
        }

        var builder = new StringBuilder();
        foreach (var error in result.Errors)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append(error.Description);
        }

        return builder.ToString();
    }
}
