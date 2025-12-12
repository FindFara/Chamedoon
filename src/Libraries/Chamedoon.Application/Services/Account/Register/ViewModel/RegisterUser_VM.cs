using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Register.ViewModel;

public class RegisterUser_VM : IMapFrom<User>
{
    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
    public string PhoneNumber { get; set; }

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "کد تایید")]
    [MaxLength(6, ErrorMessage = "{0} باید 6 رقم باشد")]
    [MinLength(6, ErrorMessage = "{0} باید 6 رقم باشد")]
    public string? VerificationCode { get; set; }
}
