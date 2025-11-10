using System.ComponentModel.DataAnnotations;
using ChamedoonWebUI.Areas.Admin.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class UsersIndexViewModel
{
    public IEnumerable<AdminUser> Users { get; set; } = Enumerable.Empty<AdminUser>();
    public IEnumerable<RoleDefinition> Roles { get; set; } = Enumerable.Empty<RoleDefinition>();
    public string? SearchTerm { get; set; }
    public Guid? SelectedRoleId { get; set; }
}

public class UserEditViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "نام کامل را وارد کنید.")]
    [Display(Name = "نام کامل")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل را وارد کنید.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "نقش کاربر را انتخاب کنید.")]
    [Display(Name = "نقش")]
    public Guid RoleId { get; set; }

    [Display(Name = "فعال است؟")]
    public bool IsActive { get; set; } = true;

    public IEnumerable<RoleDefinition> Roles { get; set; } = Enumerable.Empty<RoleDefinition>();
}
