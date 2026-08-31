using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Interaction;

public class ClassAttendance : BaseEntity
{
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime? JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public int? DurationSeconds { get; set; }
    public ParticipationLevel ParticipationLevel { get; set; }

    // Navigation properties
    public LiveClass? Class { get; set; }
    public Student? Student { get; set; }
}

public enum ParticipationLevel
{
    Silent = 1,
    Minimal = 2,
    Active = 3,
    HighlyActive = 4
}