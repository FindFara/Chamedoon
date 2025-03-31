using System.ComponentModel.DataAnnotations;

namespace ChamedoonWebUI.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور با رمز جدید مطابقت ندارد.")]
        public string ConfirmPassword { get; set; }
    }
}
