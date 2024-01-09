using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Permissions;

namespace Chamedoon.Application.Services.Permissions.ViewModel;
public class Role_VM : IMapFrom<Role>
{
    public string RoleTitle { get; set; }
}
