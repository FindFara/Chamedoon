using Chamedoon.Domin.Entity.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Countries
{
    public class CountryRestrictionConfigs : IEntityTypeConfiguration<CountryRestriction>
    {
        public void Configure(EntityTypeBuilder<CountryRestriction> builder)
        {
            builder.Property(r => r.Description).HasMaxLength(1024);
        }
    }
}
