using Chamedoon.Domin.Entity.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Payments;

public class PaymentResponseConfigs : IEntityTypeConfiguration<PaymentResponse>
{
    public void Configure(EntityTypeBuilder<PaymentResponse> builder)
    {
        builder.Property(r => r.Type).HasMaxLength(32);
        builder.Property(r => r.Message).HasMaxLength(256);
        builder.Property(r => r.GatewayTrackId).HasMaxLength(64);
        builder.Property(r => r.ReferenceId).HasMaxLength(128);
        builder.Property(r => r.CardNumber).HasMaxLength(64);

        builder.HasOne(r => r.PaymentRequest)
            .WithMany(p => p.Responses)
            .HasForeignKey(r => r.PaymentRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.PaymentRequestId);
    }
}

