using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.User;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Admin.UserManagement.ViewModel;

public class AdminEditUser_VM : IMapFrom<User>
{
    public long Id { get; set; }

    [Display(Name = "ایمیل")]
    public string Email { get; set; }

    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Display(Name = "وضعیت کاربر")]
    public bool LockoutEnabled { get; set; }
}
