using Shaghaf.Domain.Entities.Base;

namespace Shaghaf.Domain.Entities.Assessment;

public class Answer : BaseEntity
{
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsCorrect { get; set; } = false;
    public int Order { get; set; }

    // Navigation properties
    public Question? Question { get; set; }
    public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}