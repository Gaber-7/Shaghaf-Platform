using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Interaction;

public class LiveClass : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledStartTime { get; set; }
    public DateTime ScheduledEndTime { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public bool IsRecorded { get; set; } = false;
    public string? RecordingUrl { get; set; }
    public LiveClassStatus Status { get; set; }

    // Navigation properties
    public Teacher? Teacher { get; set; }
    public Course? Course { get; set; }
    public ICollection<ClassAttendance> Attendance { get; set; } = new List<ClassAttendance>();
}

public enum LiveClassStatus
{
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}