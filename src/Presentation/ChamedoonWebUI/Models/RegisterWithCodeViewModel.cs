using System.ComponentModel.DataAnnotations;

namespace ChamedoonWebUI.Models
{
    public class RegisterWithCodeViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کن.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست.")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string Email { get; set; } = string.Empty;

        public bool CodeSent { get; set; }

        public string[] CodeDigits { get; set; } = new string[4];

        [Display(Name = "رمز عبور")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} باید حداقل {2} کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "تایید رمز عبور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمزهای عبور مطابقت ندارند.")]
        public string? ConfirmPassword { get; set; }
    }
}
