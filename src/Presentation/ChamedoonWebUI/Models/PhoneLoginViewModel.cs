using System.ComponentModel.DataAnnotations;

namespace ChamedoonWebUI.Models;

public class PhoneLoginViewModel
{
    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [RegularExpression(@"^(\+98|0098|0)?9\d{9}$", ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "کد تایید")]
    [StringLength(5, MinimumLength = 5, ErrorMessage = "کد تایید باید ۵ رقم باشد")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "کد تایید باید فقط شامل اعداد باشد")]
    public string? Code { get; set; }

    public bool CodeSent { get; set; }
    public string? AlertMessage { get; set; }
}
