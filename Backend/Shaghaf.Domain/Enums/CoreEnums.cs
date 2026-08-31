namespace Shaghaf.Domain.Enums;

/// <summary>
/// User roles in the system
/// </summary>
public enum UserRole
{
    SuperAdmin = 1,
    Admin = 2,
    Teacher = 3,
    Student = 4,
    Parent = 5
}

/// <summary>
/// Education stage types
/// </summary>
public enum EducationStageType
{
    Primary = 1,
    Preparatory = 2,
    Secondary = 3
}

/// <summary>
/// Difficulty level for courses and questions
/// </summary>
public enum DifficultyLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}

/// <summary>
/// Lesson types
/// </summary>
public enum LessonType
{
    Video = 1,
    Article = 2,
    Activity = 3,
    Interactive = 4
}

/// <summary>
/// Payment status
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Successful = 2,
    Failed = 3,
    Refunded = 4
}

/// <summary>
/// Subscription status
/// </summary>
public enum SubscriptionStatus
{
    Active = 1,
    Cancelled = 2,
    Expired = 3,
    Suspended = 4
}

/// <summary>
/// Subscription tier
/// </summary>
public enum SubscriptionTier
{
    Basic = 1,
    Premium = 2,
    PremiumPlus = 3
}

/// <summary>
/// Subscription type
/// </summary>
public enum SubscriptionType
{
    Monthly = 1,
    Yearly = 2,
    OneTime = 3
}
