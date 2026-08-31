using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Assessment;

public class Submission : BaseEntity
{
    public Guid AssignmentId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? Score { get; set; }
    public string? Feedback { get; set; }
    public Guid? GradedByTeacherId { get; set; }
    public DateTime? GradedAt { get; set; }
    public bool IsLate { get; set; } = false;

    // Navigation properties
    public Assignment? Assignment { get; set; }
    public Student? Student { get; set; }
    public Teacher? GradedByTeacher { get; set; }
    public ICollection<SubmissionFile> Files { get; set; } = new List<SubmissionFile>();
}