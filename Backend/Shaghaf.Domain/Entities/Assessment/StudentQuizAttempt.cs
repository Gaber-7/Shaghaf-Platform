using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Assessment;

public class StudentQuizAttempt : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid QuizId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? Score { get; set; }
    public decimal? ScorePercentage { get; set; }
    public bool? Passed { get; set; }
    public int AttemptNumber { get; set; } = 1;
    public int? TimeSpentSeconds { get; set; }

    // Navigation properties
    public Student? Student { get; set; }
    public Quiz? Quiz { get; set; }
    public ICollection<StudentAnswer> Answers { get; set; } = new List<StudentAnswer>();
}