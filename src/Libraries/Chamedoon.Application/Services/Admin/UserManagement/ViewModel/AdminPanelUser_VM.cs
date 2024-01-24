using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.User;
using System.ComponentModel.DataAnnotations;
using Chamedoon.Application.Common.Extensions;


namespace Chamedoon.Application.Services.Admin.UserManagement.ViewModel
{
    public class AdminPanelUser_VM : IMapFrom<User>
    {
        [Display(Name = "شناسه")]
        public long? Id { get; set; }

        [Display(Name = "ایمیل")]
        public string? Email { get; set; }

        [Display(Name = "نام کاربری")]
        public string? UserName { get; set; }

        [Display(Name = "وضعیت کاربر")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "تاریخ مسدودیت")]
        public DateTime? LockoutEnd { get; set; }

        [Display(Name = " تاریخ مسدودیت شمسی")]
        public string ShamsiLockoutEnd { get => LockoutEnd.ConvertMiladiToShamsi();}

    }
}
