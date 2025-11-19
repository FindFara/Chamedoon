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

            builder.Property(c => c.UsedEvaluations)
                .HasDefaultValue(0);
        }
    }
}
