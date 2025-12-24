using Chamedoon.Domin.Entity.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Subscriptions;

public class SubscriptionPlanConfigs : IEntityTypeConfiguration<SubscriptionPlanEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlanEntity> builder)
    {
        builder.ToTable("SubscriptionPlans");

        builder.HasKey(plan => plan.Id);
        builder.Property(plan => plan.Id).HasMaxLength(64);
        builder.Property(plan => plan.Title).HasMaxLength(200).IsRequired();
        builder.Property(plan => plan.DurationLabel).HasMaxLength(64).IsRequired();
        builder.Property(plan => plan.OriginalPrice).IsRequired();
        builder.Property(plan => plan.Price).IsRequired();
        builder.Property(plan => plan.FeaturesJson).HasMaxLength(4000);
        builder.Property(plan => plan.IsActive).HasDefaultValue(true);
        builder.Property(plan => plan.SortOrder).HasDefaultValue(0);
        builder.Property(plan => plan.CreatedAtUtc).IsRequired();
        builder.Property(plan => plan.UpdatedAtUtc);
    }
}
