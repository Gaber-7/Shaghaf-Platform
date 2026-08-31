using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Assessment;

public class Quiz : BaseEntity
{
    public Guid CourseId { get; set; }
    public Guid? LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PassingScore { get; set; } = 70;
    public int? TimeLimitMinutes { get; set; }
    public bool RandomizeQuestions { get; set; } = false;
    public bool ShuffleAnswers { get; set; } = true;
    public bool AllowRetakes { get; set; } = true;
    public int? MaxAttempts { get; set; }
    public bool IsPublished { get; set; } = false;

    // Navigation properties
    public Course? Course { get; set; }
    public Lesson? Lesson { get; set; }
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<StudentQuizAttempt> Attempts { get; set; } = new List<StudentQuizAttempt>();
}