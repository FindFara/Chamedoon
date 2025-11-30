using Chamedoon.Domin.Entity.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Countries
{
    public class CountryConfigs : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasIndex(c => c.Key).IsUnique();

            builder.Property(c => c.Key).HasMaxLength(64).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(128).IsRequired();
            builder.Property(c => c.InvestmentCurrency).HasMaxLength(64);

            builder.HasMany(c => c.LivingCosts)
                .WithOne(l => l.Country)
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Restrictions)
                .WithOne(r => r.Country)
                .HasForeignKey(r => r.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
