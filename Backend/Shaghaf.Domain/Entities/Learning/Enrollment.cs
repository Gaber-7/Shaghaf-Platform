using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Learning;

public class Enrollment : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal Progress { get; set; } = 0;
    public int TotalLessonCount { get; set; }
    public int CompletedLessonCount { get; set; }

    // Navigation properties
    public Student? Student { get; set; }
    public Course? Course { get; set; }
}