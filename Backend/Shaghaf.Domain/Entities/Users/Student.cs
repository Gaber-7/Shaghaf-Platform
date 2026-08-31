using Shaghaf.Domain.Entities.Learning;

namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Student entity inheriting from User
/// </summary>
public class Student : User
{
    public Guid? ParentId { get; set; }
    public int? GradeId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int TotalXP { get; set; } = 0;
    public int CurrentLevel { get; set; } = 1;
    public int CurrentStreak { get; set; } = 0;
    public DateTime? LastStudyDate { get; set; }

    // Navigation properties
    public Parent? Parent { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public ICollection<StudentAchievement> Achievements { get; set; } = new List<StudentAchievement>();
}
