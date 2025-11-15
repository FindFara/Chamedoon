using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class UsersIndexViewModel
{
    public IReadOnlyList<UserListItemViewModel> Users { get; init; } = Array.Empty<UserListItemViewModel>();
    public IReadOnlyList<RoleOptionViewModel> Roles { get; init; } = Array.Empty<RoleOptionViewModel>();
    public string? SearchTerm { get; init; }
    public long? SelectedRoleId { get; init; }
}

public class UserListItemViewModel
{
    public long Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public long? RoleId { get; init; }
    public string? RoleName { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }

    public static UserListItemViewModel FromDto(AdminUserDto dto)
        => new()
        {
            Id = dto.Id,
            Email = dto.Email,
            UserName = dto.UserName,
            FullName = dto.FullName,
            RoleId = dto.RoleId,
            RoleName = dto.RoleName,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt
        };
}

public class RoleOptionViewModel
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class UserEditViewModel : IValidatableObject
{
    public long? Id { get; set; }

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "نام کامل")]
    public string? FullName { get; set; }

    [Display(Name = "نقش")]
    public long? RoleId { get; set; }

    [Display(Name = "کاربر فعال است؟")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "کلمه عبور")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public IEnumerable<RoleOptionViewModel> Roles { get; set; } = Array.Empty<RoleOptionViewModel>();

    public bool IsNew => !Id.HasValue;

    public AdminUserInput ToInput()
        => new()
        {
            Id = Id,
            Email = Email,
            UserName = UserName,
            FullName = FullName,
            RoleId = RoleId,
            IsActive = IsActive,
            Password = Password
        };

    public static UserEditViewModel FromDto(AdminUserDto dto)
        => new()
        {
            Id = dto.Id,
            Email = dto.Email,
            UserName = dto.UserName,
            FullName = dto.FullName,
            RoleId = dto.RoleId,
            IsActive = dto.IsActive
        };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsNew && string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult("وارد کردن کلمه عبور برای کاربر جدید الزامی است.", new[] { nameof(Password) });
        }
    }
}
