using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Learning;

public class StudentAchievement : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid AchievementId { get; set; }
    public DateTime EarnedAt { get; set; }

    // Navigation properties
    public Student? Student { get; set; }
    public Achievement? Achievement { get; set; }
}