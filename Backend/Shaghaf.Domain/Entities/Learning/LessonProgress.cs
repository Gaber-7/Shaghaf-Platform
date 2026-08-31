using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Learning;

public class LessonProgress : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid LessonId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int TimeSpentSeconds { get; set; }
    public ProgressStatus Status { get; set; }
    public DateTime LastAccessedAt { get; set; }

    // Navigation properties
    public Student? Student { get; set; }
    public Lesson? Lesson { get; set; }
    public Course? Course { get; set; }
}

public enum ProgressStatus
{
    NotStarted = 1,
    InProgress = 2,
    Completed = 3
}