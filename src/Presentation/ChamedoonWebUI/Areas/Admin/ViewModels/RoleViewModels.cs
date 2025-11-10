using System.ComponentModel.DataAnnotations;
using ChamedoonWebUI.Areas.Admin.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class RolesIndexViewModel
{
    public IEnumerable<RoleDefinition> Roles { get; set; } = Enumerable.Empty<RoleDefinition>();
    public IEnumerable<PermissionDefinition> Permissions { get; set; } = Enumerable.Empty<PermissionDefinition>();
}

public class RoleEditViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "نام نقش را وارد کنید.")]
    [Display(Name = "نام نقش")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "توضیحات")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "مجوزها")]
    public List<Guid> SelectedPermissions { get; set; } = new();

    public IEnumerable<PermissionDefinition> Permissions { get; set; } = Enumerable.Empty<PermissionDefinition>();
}
