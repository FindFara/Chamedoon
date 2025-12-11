using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class RolesIndexViewModel
{
    public IReadOnlyList<RoleListItemViewModel> Roles { get; init; } = Array.Empty<RoleListItemViewModel>();
}

public class RoleListItemViewModel
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();

    public static RoleListItemViewModel FromDto(AdminRoleDto dto)
        => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Permissions = dto.PermissionNames.ToList()
        };
}

public class RoleEditViewModel
{
    public long? Id { get; set; }

    [Display(Name = "نام نقش")]
    [Required(ErrorMessage = "وارد کردن نام نقش الزامی است.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "مجوزهای نقش")]
    public ICollection<string> SelectedPermissions { get; set; } = new List<string>();

    [Display(Name = "افزودن مجوز جدید (با کاما جدا کنید)")]
    public string? AdditionalPermissions { get; set; }

    public IEnumerable<string> AvailablePermissions { get; set; } = Array.Empty<string>();

    public AdminRoleInput ToInput()
    {
        var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var permission in SelectedPermissions ?? Array.Empty<string>())
        {
            if (!string.IsNullOrWhiteSpace(permission))
            {
                permissions.Add(permission.Trim());
            }
        }

        if (!string.IsNullOrWhiteSpace(AdditionalPermissions))
        {
            var extras = AdditionalPermissions
                .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var permission in extras)
            {
                if (!string.IsNullOrWhiteSpace(permission))
                {
                    permissions.Add(permission.Trim());
                }
            }
        }

        return new AdminRoleInput
        {
            Id = Id,
            Name = Name,
            PermissionNames = permissions.ToList()
        };
    }

    public static RoleEditViewModel FromDto(AdminRoleDto dto)
        => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            SelectedPermissions = dto.PermissionNames.ToList()
        };
}
