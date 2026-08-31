using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Assessment;

public class Question : BaseEntity
{
    public Guid QuizId { get; set; }
    public Guid? QuestionBankId { get; set; }
    public string Content { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public string? ImageUrl { get; set; }
    public int Order { get; set; }
    public int Points { get; set; } = 1;
    public DifficultyLevel DifficultyLevel { get; set; }
    public string? ExplanationText { get; set; }

    // Navigation properties
    public Quiz? Quiz { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}