using Chamedoon.Domin.Entity.User;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Domin.Entity.Permissions;

public class Role : IdentityRole<long>
{
    #region Relations
    public ICollection<RolePermission> RolePermissions { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }

    #endregion
}

