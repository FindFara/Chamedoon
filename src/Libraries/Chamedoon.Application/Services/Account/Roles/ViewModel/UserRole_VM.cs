using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.User;

namespace Chamedoon.Application.Services.Account.Roles.ViewModel;
public class UserRole_VM : IMapFrom<UserRole>
{
    public int UR { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public bool IsDeleted { get; set; }
}
