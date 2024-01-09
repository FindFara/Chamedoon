using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.User;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        #region User
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        #endregion

        #region Permission
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
