using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Engagement;

/// <summary>
/// Pre-computed engagement score for a teacher over a period, with the
/// component breakdown kept so a ranking can be explained to users.
/// </summary>
public class TeacherEngagementScore : BaseEntity
{
    public Guid TeacherId { get; set; }
    public EngagementPeriod Period { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal TotalScore { get; set; }
    public decimal VideoEngagementScore { get; set; }
    public decimal StudentInteractionScore { get; set; }
    public decimal QnaActivityScore { get; set; }
    public decimal LiveClassActivityScore { get; set; }
    public decimal CourseCompletionScore { get; set; }
    public decimal RatingScore { get; set; }
    public decimal StudentRetentionScore { get; set; }
    public int UniqueStudents { get; set; }
    public long TotalWatchTimeSeconds { get; set; }
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Teacher? Teacher { get; set; }
}

/// <summary>
/// Configurable weights used when computing <see cref="TeacherEngagementScore"/>.
/// </summary>
public class EngagementWeightConfig : BaseEntityInt
{
    public string Component { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public bool IsActive { get; set; } = true;
}
