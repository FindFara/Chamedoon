using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;

namespace Chamedoon.Application.Services.Account.Users.ViewModel;
public class EditUser_VM : IMapFrom<User>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }

}
