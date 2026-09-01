using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Payments;

public class SubscriptionPlan : BaseEntityInt
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SubscriptionTier Tier { get; set; }
    public SubscriptionType Type { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "EGP";
    public int DurationDays { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}

public class Subscription : BaseEntity
{
    public Guid StudentId { get; set; }
    public int PlanId { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool AutoRenew { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Navigation properties
    public Student? Student { get; set; }
    public SubscriptionPlan? Plan { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
