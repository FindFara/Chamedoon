using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Login.ViewModel;

public class VerifyTwoFactorCodeViewModel
{
    [Required]
    public long UserId { get; set; }

    [Display(Name = "کد تایید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(6, ErrorMessage = "{0} باید 6 رقم باشد")]
    [MinLength(6, ErrorMessage = "{0} باید 6 رقم باشد")]
    public string Code { get; set; }

    public bool RememberMe { get; set; }
}
