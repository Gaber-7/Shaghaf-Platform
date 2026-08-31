using Shaghaf.Domain.Entities.Base;

namespace Shaghaf.Domain.Entities.Courses;

public class CourseSection : BaseEntity
{
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }

    // Navigation properties
    public Course? Course { get; set; }
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}