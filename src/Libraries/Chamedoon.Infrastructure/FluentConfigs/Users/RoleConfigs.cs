using Chamedoon.Domin.Entity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Chamedoon.Infrastructure.FluentConfigs.Users;

public class RoleConfigs : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
        List<Role> roles = new List<Role>()
        {
           new Role {Id= 1, Name = "Admin", NormalizedName = "ADMIN" },
           new Role {Id= 2, Name = "Member", NormalizedName = "MEMBER" },
           new Role {Id= 3, Name = "Manager", NormalizedName = "MANAGER" }
        };

        builder.HasData(roles);
    }
}
