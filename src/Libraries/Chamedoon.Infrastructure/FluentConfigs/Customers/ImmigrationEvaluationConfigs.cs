using Chamedoon.Domin.Entity.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Customers
{
    public class ImmigrationEvaluationConfigs : IEntityTypeConfiguration<ImmigrationEvaluation>
    {
        public void Configure(EntityTypeBuilder<ImmigrationEvaluation> builder)
        {
            builder.Property(evaluation => evaluation.JobTitle)
                .HasMaxLength(256);

            builder.Property(evaluation => evaluation.InvestmentAmount)
                .HasPrecision(18, 2);

            builder.Property(evaluation => evaluation.PhoneNumber)
                .HasMaxLength(32);

            builder.Property(evaluation => evaluation.Notes)
                .HasMaxLength(1000);

            builder.HasOne(evaluation => evaluation.Customer)
                .WithMany(customer => customer.ImmigrationEvaluations)
                .HasForeignKey(evaluation => evaluation.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(evaluation => evaluation.CustomerId);
            builder.HasIndex(evaluation => evaluation.CreatedAtUtc);
        }
    }
}
