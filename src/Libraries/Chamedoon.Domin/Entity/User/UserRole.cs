using Chamedoon.Domin.Entity.Permissions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Domin.Entity.User;

public class UserRole : IdentityUserRole<long>
{
    public User User { get; set; }
    public Role Role { get; set; }
}

