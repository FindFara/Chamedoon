using Chamedoon.Domin.Entity.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Payments;

public class DiscountCodeConfigs : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(c => c.Description)
            .HasMaxLength(512);

        builder.HasIndex(c => c.Code).IsUnique();
    }
}
