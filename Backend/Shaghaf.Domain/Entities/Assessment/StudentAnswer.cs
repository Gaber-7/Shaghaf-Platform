using Shaghaf.Domain.Entities.Base;

namespace Shaghaf.Domain.Entities.Assessment;

public class StudentAnswer : BaseEntity
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
    public string? TextAnswer { get; set; }
    public bool? IsCorrect { get; set; }
    public int? PointsEarned { get; set; }

    // Navigation properties
    public StudentQuizAttempt? Attempt { get; set; }
    public Question? Question { get; set; }
    public Answer? Answer { get; set; }
}