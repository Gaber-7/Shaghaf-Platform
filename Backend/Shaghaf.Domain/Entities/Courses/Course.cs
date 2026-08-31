using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Education;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Courses;

public class Course : BaseEntity
{
    public Guid TeacherId { get; set; }
    public int GradeId { get; set; }
    public int SubjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public decimal Price { get; set; } = 0;
    public bool IsFree { get; set; } = false;
    public bool IsPublished { get; set; } = false;
    public decimal Rating { get; set; } = 0;
    public int StudentCount { get; set; } = 0;
    public int LessonCount { get; set; } = 0;
    public int TotalDurationSeconds { get; set; } = 0;

    // Navigation properties
    public Teacher? Teacher { get; set; }
    public Grade? Grade { get; set; }
    public Subject? Subject { get; set; }
    public ICollection<CourseSection> Sections { get; set; } = new List<CourseSection>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();
}