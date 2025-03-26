using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Users.ViewModel;

public class ChangePasswordViewModel
{
    public string? UserId { get; set; }
    public required string NewPassword { get; set; }
    public required string CurrentPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "کلمه های عبور مغایرت دارند")]
    public required string RePassword { get; set; }
}
