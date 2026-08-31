using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Interaction;

public class LessonQuestion : BaseEntity
{
    public Guid LessonId { get; set; }
    public Guid StudentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsResolved { get; set; } = false;
    public Guid? AcceptedReplyId { get; set; }
    public int UpvoteCount { get; set; } = 0;

    // Navigation properties
    public Lesson? Lesson { get; set; }
    public Student? Student { get; set; }
    public ICollection<LessonReply> Replies { get; set; } = new List<LessonReply>();
}