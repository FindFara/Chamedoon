namespace Chamedoon.Application.Services.Account.Register.ViewModel;
public class ResponseRegisterUser_VM
{
    public string? Message { get; set; }
    public int Code { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
