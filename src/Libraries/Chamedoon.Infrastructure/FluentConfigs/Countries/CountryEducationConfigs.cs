using Chamedoon.Domin.Entity.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Countries
{
    public class CountryEducationConfigs : IEntityTypeConfiguration<CountryEducation>
    {
        public void Configure(EntityTypeBuilder<CountryEducation> builder)
        {
            builder.Property(e => e.FieldName).HasMaxLength(256).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(2048);
            builder.Property(e => e.Level).HasMaxLength(128);
            builder.Property(e => e.LanguageRequirement).HasMaxLength(1024);

            builder.HasOne(e => e.Country)
                .WithMany(c => c.Educations)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
