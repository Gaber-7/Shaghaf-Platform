using Shaghaf.Domain.Entities.Base;

namespace Shaghaf.Domain.Entities.Assessment;

public class SubmissionFile : BaseEntity
{
    public Guid SubmissionId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Submission? Submission { get; set; }
}