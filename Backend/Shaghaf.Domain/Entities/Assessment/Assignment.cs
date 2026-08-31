using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;

namespace Shaghaf.Domain.Entities.Assessment;

public class Assignment : BaseEntity
{
    public Guid CourseId { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPublished { get; set; } = false;
    public string? RubricJson { get; set; }

    // Navigation properties
    public Course? Course { get; set; }
    public Lesson? Lesson { get; set; }
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}