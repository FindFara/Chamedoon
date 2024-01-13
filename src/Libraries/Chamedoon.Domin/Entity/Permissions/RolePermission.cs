using Chamedoon.Domin.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chamedoon.Domin.Entity.Permissions;

public class RolePermission : BaseEntity
{
    public long? RoleId { get; set; }
    public string? PermissionTitle { get; set; }
    public string? PermissionName { get; set; }
    public long? ParentId { get; set; }

    #region Relations

    [ForeignKey("ParentId")]
    public List<RolePermission>? Permissions { get; set; }
    public Role? Roles { get; set; }

    #endregion

}

