using Chamedoon.Application.Common.Extensions;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Users;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Admin.UserManagement.ViewModel;

public class AdminUserDetaile_VM : IMapFrom<User>, IMapFrom<AdminCreateOrEditUser_VM>
{
    public long Id { get; set; }

    [Display(Name = "ایمیل")]
    public string Email { get; set; }

    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Display(Name = "وضعیت کاربر")]
    public bool LockoutEnabled { get; set; }

    [Display(Name = "تاریخ مسدودیت")]
    public DateTime? LockoutEnd { get; set; }

    [Display(Name = " تاریخ مسدودیت شمسی")]
    public string? ShamsiLockoutEnd { get => LockoutEnd.ConvertMiladiToShamsi(); }

    [Display(Name = "تاریخ ثبت نام")]
    public DateTime Created { get; set; }

    [Display(Name = "تاریخ بروزرسانی")]
    public DateTime? LastModified { get; set; }

    #region Reletion
    public List<Role> PermissionList { get; set; }

    #endregion
}
