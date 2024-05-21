using Chamedoon.Domin.Entity.Users;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Domin.Entity.Permissions;

public class Role : IdentityRole<long>
{
    #region Relations
    public List<RolePermission>? RolePermissions { get; set; }
    public List<UserRole>? UserRoles { get; set; }

    #endregion
}

