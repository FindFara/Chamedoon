using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Chamedoon.Application.Services.Admin.UserManagement.ViewModel
{
    public class AdminPanelUser_VM
    {
        [Display(Name = "شناسه")]
        public long? Id { get; set; }

        [Display(Name = "ایمیل")]
        public string? Email { get; set; }
        [Display(Name = "نام کاربری")]
        public string? UserName { get; set; }

    }

}
