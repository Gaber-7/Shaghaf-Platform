using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Domain.Entities.Media;

public class VideoWatchSession : BaseEntity
{
    public Guid VideoId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int SessionDurationSeconds { get; set; }
    public int VideoDurationWatchedSeconds { get; set; }
    public int LastWatchedPositionSeconds { get; set; }
    public decimal WatchPercentage { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public int RewatchCount { get; set; } = 0;

    // Navigation properties
    public Video? Video { get; set; }
    public Student? Student { get; set; }
}