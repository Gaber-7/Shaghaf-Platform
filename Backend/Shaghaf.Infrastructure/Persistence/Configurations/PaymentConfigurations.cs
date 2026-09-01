using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Payments;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("SubscriptionPlans");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        builder.Property(p => p.Price).HasPrecision(10, 2);
        builder.Property(p => p.Tier).HasConversion<int>();
        builder.Property(p => p.Type).HasConversion<int>();

        builder.HasIndex(p => new { p.Tier, p.Type }).IsUnique();
    }
}

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");
        builder.Property(s => s.Status).HasConversion<int>();

        builder.HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Plan)
            .WithMany(p => p!.Subscriptions)
            .HasForeignKey(s => s.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.StudentId, s.Status });
        builder.HasIndex(s => s.EndsAt);
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.Property(p => p.Amount).HasPrecision(10, 2);
        builder.Property(p => p.DiscountAmount).HasPrecision(10, 2);
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        builder.Property(p => p.Provider).IsRequired().HasMaxLength(50);
        builder.Property(p => p.ProviderTransactionId).HasMaxLength(150);
        builder.Property(p => p.InvoiceNumber).HasMaxLength(50);
        builder.Property(p => p.Status).HasConversion<int>();

        builder.HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Course)
            .WithMany()
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Subscription)
            .WithMany(s => s!.Payments)
            .HasForeignKey(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Coupon)
            .WithMany(c => c!.Payments)
            .HasForeignKey(p => p.CouponId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.Provider, p.ProviderTransactionId }).IsUnique().HasFilter("[ProviderTransactionId] IS NOT NULL");
        builder.HasIndex(p => new { p.StudentId, p.Status });
        builder.HasIndex(p => p.PaidAt);
    }
}

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");
        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);
        builder.Property(c => c.PercentageOff).HasPrecision(5, 2);
        builder.Property(c => c.AmountOff).HasPrecision(10, 2);

        builder.HasIndex(c => c.Code).IsUnique();
    }
}
