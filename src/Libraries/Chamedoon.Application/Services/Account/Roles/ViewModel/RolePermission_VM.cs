using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Permissions;

namespace Chamedoon.Application.Services.Account.Roles.ViewModel;
public class RolePermission_VM : IMapFrom<RolePermission>
{
    public int RoleId { get; set; }
    public string PermissionTitle { get; set; }
    public string PermissionName { get; set; }
    public long? ParentId { get; set; }

}
