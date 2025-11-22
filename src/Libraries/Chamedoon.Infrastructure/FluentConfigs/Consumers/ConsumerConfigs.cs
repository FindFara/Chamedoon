using Chamedoon.Domin.Entity.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Chamedoon.Infrastructure.FluentConfigs.Customers
{
    public class CustomerConfigs : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.SubscriptionPlanId)
                .HasMaxLength(64);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(32);

            builder.Property(c => c.MbtiType)
                .HasMaxLength(8);

            builder.Property(c => c.LanguageCertificate)
                .HasMaxLength(128);

            builder.Property(c => c.FieldCategory)
                .HasMaxLength(256);

            builder.Property(c => c.InvestmentAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.UsedEvaluations)
                .HasDefaultValue(0);
        }
    }
}
