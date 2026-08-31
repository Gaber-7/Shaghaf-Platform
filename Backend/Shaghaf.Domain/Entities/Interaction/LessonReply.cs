using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Interaction;

public class LessonReply : BaseEntity
{
    public Guid QuestionId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsAcceptedAnswer { get; set; } = false;
    public int UpvoteCount { get; set; } = 0;

    // Navigation properties
    public LessonQuestion? Question { get; set; }
    public User? Author { get; set; }
}