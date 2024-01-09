using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.User;

namespace Chamedoon.Application.Services.Account.ViewModel;

public class UserDetails_VM : IMapFrom<User>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool? IsBlocked { get; set; }
    public bool IsEmailAtive { get; set; }
    public string EmailActiveCode { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? LastModified { get; set; }
}
