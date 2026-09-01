using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Certificates;

public class Certificate : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid TeacherId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? FileUrl { get; set; }
    public CertificateStatus Status { get; set; } = CertificateStatus.Generated;

    // Navigation properties
    public Student? Student { get; set; }
    public Course? Course { get; set; }
    public Teacher? Teacher { get; set; }
}
