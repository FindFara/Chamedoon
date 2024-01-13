using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Domin.Entity.User;

public class User : IdentityUser<long>
{
    public bool IsBlocked { get; set; }
    public List<UserRole>? UserRoles { get; set; }

}
