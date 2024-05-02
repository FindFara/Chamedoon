using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.User;
using Chamedoon.Infrastructure.FluentConfigs.Users;
using CodeTo.Domain.Entities.Articles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<
                                    User,
                                    Role,
                                    long,
                                    IdentityUserClaim<long>,
                                    UserRole,
                                    IdentityUserLogin<long>,
                                    IdentityRoleClaim<long>,
                                    IdentityUserToken<long>>,
                                    IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    #region User
    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    #endregion

    #region Permission
    public DbSet<Role> Role { get; set; }
    public DbSet<RolePermission> RolePermission { get; set; }
    #endregion

    #region Blog
    public DbSet<Article> Article { get; set; }
    public DbSet<ArticleComment> ArticleComment { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<User>().HasQueryFilter(u => !u.LockoutEnabled);

        modelBuilder.Entity<User>(b =>
        {
            b.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<Role>(b =>
        {
            b.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });
        modelBuilder.ApplyConfiguration(new UserConfigs());
        modelBuilder.ApplyConfiguration(new UserRoleConfigs());
        modelBuilder.ApplyConfiguration(new RoleConfigs());
        modelBuilder.ApplyConfiguration(new RolePermissionConfigs());

    }
}