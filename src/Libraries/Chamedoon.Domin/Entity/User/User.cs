using Chamedoon.Domin.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Domin.Entity.User;

public class User : IdentityUser<long> , IAuditableEntity
{
    public List<UserRole>? UserRoles { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
