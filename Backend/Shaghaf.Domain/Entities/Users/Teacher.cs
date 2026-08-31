using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Engagement;
using Shaghaf.Domain.Entities.Interaction;
using Shaghaf.Domain.Entities.Reviews;

namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Teacher entity inheriting from User
/// </summary>
public class Teacher : User
{
    public string? Biography { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Qualifications { get; set; }
    public decimal? Rating { get; set; } = 0;
    public int? ReviewCount { get; set; } = 0;
    public bool IsVerified { get; set; } = false;
    public bool IsFeatured { get; set; } = false;

    // Navigation properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<TeacherReview> Reviews { get; set; } = new List<TeacherReview>();
    public ICollection<LiveClass> LiveClasses { get; set; } = new List<LiveClass>();
    public ICollection<LessonReply> Replies { get; set; } = new List<LessonReply>();
    public TeacherEngagementScore? EngagementScore { get; set; }
}
