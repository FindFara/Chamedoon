using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Domin.Entity.User;

public class User : IdentityUser<long>
{
    public List<UserRole>? UserRoles { get; set; }

}
