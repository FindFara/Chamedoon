using Chamedoon.Domin.Entity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Users;

public class RoleConfigs : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
    }
}
