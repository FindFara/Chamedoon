using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Login.ViewModel
{
    public class LoginUserViewModel : IMapFrom<User>
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
