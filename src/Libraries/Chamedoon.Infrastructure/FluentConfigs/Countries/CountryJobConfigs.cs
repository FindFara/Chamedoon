using Chamedoon.Domin.Entity.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Countries
{
    public class CountryJobConfigs : IEntityTypeConfiguration<CountryJob>
    {
        public void Configure(EntityTypeBuilder<CountryJob> builder)
        {
            builder.Property(j => j.Title).HasMaxLength(256).IsRequired();
            builder.Property(j => j.Description).HasMaxLength(2048);
            builder.Property(j => j.ExperienceImpact).HasMaxLength(1024);

            builder.HasOne(j => j.Country)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
