using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Payments;

/// <summary>
/// A payment for either a course purchase or a subscription. The provider is
/// stored as a string so additional payment providers can be plugged in later.
/// </summary>
public class Payment : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public int? CouponId { get; set; }
    public decimal Amount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Currency { get; set; } = "EGP";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string Provider { get; set; } = string.Empty;
    public string? ProviderTransactionId { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? RefundedAt { get; set; }

    // Navigation properties
    public Student? Student { get; set; }
    public Course? Course { get; set; }
    public Subscription? Subscription { get; set; }
    public Coupon? Coupon { get; set; }
}

public class Coupon : BaseEntityInt
{
    public string Code { get; set; } = string.Empty;
    public decimal? PercentageOff { get; set; }
    public decimal? AmountOff { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? MaxRedemptions { get; set; }
    public int RedemptionCount { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
