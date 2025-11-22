using Chamedoon.Domin.Entity.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Customers
{
    public class CustomerReportConfigs : IEntityTypeConfiguration<CustomerReport>
    {
        public void Configure(EntityTypeBuilder<CustomerReport> builder)
        {
            builder.Property(r => r.MbtiType).HasMaxLength(8);
            builder.Property(r => r.LanguageCertificate).HasMaxLength(128);
            builder.Property(r => r.FieldCategory).HasMaxLength(256);
            builder.Property(r => r.PhoneNumber).HasMaxLength(32);

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
