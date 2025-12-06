using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Register.ViewModel;

public enum RegisterStep
{
    EnterEmail = 0,
    VerifyCode = 1,
    SetCredentials = 2
}

public class RegisterUser_VM : IMapFrom<User>
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
    public string Email { get; set; }

    [Display(Name = "کد تایید")]
    [MaxLength(4, ErrorMessage = "{0} باید چهار رقمی باشد.")]
    [MinLength(4, ErrorMessage = "{0} باید چهار رقمی باشد.")]
    public string? VerificationCode { get; set; }

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? UserName { get; set; }

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد.")]
    public string? Password { get; set; }

    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن یکسان نیست.")]
    public string? ConfirmPassword { get; set; }

    public RegisterStep Stage { get; set; } = RegisterStep.EnterEmail;
}
