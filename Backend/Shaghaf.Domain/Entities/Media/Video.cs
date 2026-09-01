using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;

namespace Shaghaf.Domain.Entities.Media;

public class Video : BaseEntity
{
    public Guid LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Provider { get; set; } = "Custom"; // YouTube, Vimeo, Custom, etc.
    public int DurationSeconds { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsPublished { get; set; } = false;

    // Navigation properties
    public Lesson? Lesson { get; set; }
    public VideoAnalytics? Analytics { get; set; }
    public ICollection<VideoWatchSession> WatchSessions { get; set; } = new List<VideoWatchSession>();
}