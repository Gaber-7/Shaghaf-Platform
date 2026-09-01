using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Reviews;

public class CourseReview : BaseEntity
{
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public int Rating { get; set; }
    public string? Content { get; set; }
    public int HelpfulCount { get; set; }
    public bool IsApproved { get; set; } = true;

    // Navigation properties
    public Course? Course { get; set; }
    public Student? Student { get; set; }
}
