using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Reviews;

public class TeacherReview : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Guid StudentId { get; set; }
    public int Rating { get; set; }
    public string? Content { get; set; }
    public int HelpfulCount { get; set; }
    public bool IsApproved { get; set; } = true;

    // Navigation properties
    public Teacher? Teacher { get; set; }
    public Student? Student { get; set; }
}
