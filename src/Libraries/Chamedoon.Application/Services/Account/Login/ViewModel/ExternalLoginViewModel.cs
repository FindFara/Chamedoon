namespace Chamedoon.Application.Services.Account.Login.ViewModel;
public class ExternalLoginViewModel
{
    public bool IsSuccess { get; set; }
    public string RedirectUrl { get; set; }
    public string LoginProvider { get; set; }
    public string Email { get; set; }
}
