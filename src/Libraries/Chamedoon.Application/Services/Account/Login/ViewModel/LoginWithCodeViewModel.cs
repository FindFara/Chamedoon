using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Login.ViewModel;

public class LoginWithCodeViewModel
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

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
}
