using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.Blogs;
using Chamedoon.Domin.Entity.Countries;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Infrastructure.FluentConfigs.Customers;
using Chamedoon.Infrastructure.FluentConfigs.Countries;
using Chamedoon.Infrastructure.FluentConfigs.Payments;
using Chamedoon.Infrastructure.FluentConfigs.Users;
using Chamedoon.Infrastructure.Persistence.Seeds;
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

    #region Customer
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ImmigrationEvaluation> ImmigrationEvaluations { get; set; }
    #endregion

    #region Countries
    public DbSet<Country> Countries { get; set; }
    public DbSet<CountryLivingCost> CountryLivingCosts { get; set; }
    public DbSet<CountryRestriction> CountryRestrictions { get; set; }
    #endregion

    #region Payments
    public DbSet<PaymentRequest> PaymentRequests { get; set; }
    public DbSet<PaymentResponse> PaymentResponses { get; set; }
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
        modelBuilder.ApplyConfiguration(new CustomerConfigs());
        modelBuilder.ApplyConfiguration(new ImmigrationEvaluationConfigs());
        modelBuilder.ApplyConfiguration(new PaymentRequestConfigs());
        modelBuilder.ApplyConfiguration(new PaymentResponseConfigs());
        modelBuilder.ApplyConfiguration(new CountryConfigs());
        modelBuilder.ApplyConfiguration(new CountryLivingCostConfigs());
        modelBuilder.ApplyConfiguration(new CountryRestrictionConfigs());

        CountrySeedData.Seed(modelBuilder);


    }
}