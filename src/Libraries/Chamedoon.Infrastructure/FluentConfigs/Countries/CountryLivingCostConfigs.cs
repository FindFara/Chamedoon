using Chamedoon.Domin.Entity.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Countries
{
    public class CountryLivingCostConfigs : IEntityTypeConfiguration<CountryLivingCost>
    {
        public void Configure(EntityTypeBuilder<CountryLivingCost> builder)
        {
            builder.Property(l => l.Value).HasMaxLength(1024);
        }
    }
}
