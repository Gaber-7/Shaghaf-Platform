using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Courses;

public class Lesson : BaseEntity
{
    public Guid SectionId { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public LessonType Type { get; set; }
    public bool IsPreview { get; set; } = false;
    public bool IsPublished { get; set; } = false;

    // Navigation properties
    public CourseSection? Section { get; set; }
    public Course? Course { get; set; }
    public Video? Video { get; set; }
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public ICollection<LessonQuestion> Questions { get; set; } = new List<LessonQuestion>();
}