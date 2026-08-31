namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Teacher entity
/// </summary>
public class Teacher : User
{
    public string? Biography { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Qualifications { get; set; }
    public decimal? Rating { get; set; } = 0;
    public int? ReviewCount { get; set; } = 0;
    
    // Navigation properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<TeacherReview> Reviews { get; set; } = new List<TeacherReview>();
    public ICollection<LiveClass> LiveClasses { get; set; } = new List<LiveClass>();
    public ICollection<LessonReply> Replies { get; set; } = new List<LessonReply>();
    public TeacherEngagementScore? EngagementScore { get; set; }
}
