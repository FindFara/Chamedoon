using Chamedoon.Domin.Entity.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Payments;

public class PaymentRequestConfigs : IEntityTypeConfiguration<PaymentRequest>
{
    public void Configure(EntityTypeBuilder<PaymentRequest> builder)
    {
        builder.Property(p => p.PlanId).HasMaxLength(64);
        builder.Property(p => p.Description).HasMaxLength(512);
        builder.Property(p => p.CallbackUrl).HasMaxLength(512);
        builder.Property(p => p.GatewayTrackId).HasMaxLength(64);
        builder.Property(p => p.ReferenceCode).HasMaxLength(64);
        builder.Property(p => p.DiscountCode).HasMaxLength(64);
        builder.Property(p => p.DiscountValue);
        builder.Property(p => p.DiscountAmount);
        builder.Property(p => p.FinalAmount).IsRequired();
        builder.Property(p => p.PaymentUrl).HasMaxLength(512);
        builder.Property(p => p.LastError).HasMaxLength(512);

        builder.HasOne(p => p.Customer)
            .WithMany(c => c.PaymentRequests)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.GatewayTrackId);
        builder.HasIndex(p => new { p.CustomerId, p.Status });
    }
}

