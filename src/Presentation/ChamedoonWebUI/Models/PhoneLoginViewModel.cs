using System.ComponentModel.DataAnnotations;

namespace ChamedoonWebUI.Models;

public class PhoneLoginViewModel
{
    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [RegularExpression(@"^(\+98|0098|0)?9\d{9}$", ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "کد تایید")]
    [StringLength(6, MinimumLength = 4, ErrorMessage = "کد تایید معتبر وارد کنید")]
    public string? Code { get; set; }

    public bool CodeSent { get; set; }
    public string? AlertMessage { get; set; }
}
