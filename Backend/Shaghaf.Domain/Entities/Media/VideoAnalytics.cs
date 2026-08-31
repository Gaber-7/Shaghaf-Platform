using Shaghaf.Domain.Entities.Base;

namespace Shaghaf.Domain.Entities.Media;

public class VideoAnalytics : BaseEntity
{
    public Guid VideoId { get; set; }
    public int TotalSessions { get; set; }
    public int TotalViews { get; set; }
    public int UniqueStudents { get; set; }
    public long TotalWatchTimeSeconds { get; set; }
    public decimal AverageWatchDurationSeconds { get; set; }
    public decimal AverageWatchPercentage { get; set; }
    public decimal CompletionRate { get; set; }
    public int CompletedViews { get; set; }
    public int PartiallyWatchedViews { get; set; }
    public DateTime LastUpdated { get; set; }

    // Navigation properties
    public Video? Video { get; set; }
    public ICollection<VideoAnalyticsDailySnapshot> DailySnapshots { get; set; } = new List<VideoAnalyticsDailySnapshot>();
}

public class VideoAnalyticsDailySnapshot : BaseEntity
{
    public Guid VideoAnalyticsId { get; set; }
    public DateTime Date { get; set; }
    public int Views { get; set; }
    public int UniqueStudents { get; set; }
    public long WatchTimeSeconds { get; set; }
    public decimal CompletionRate { get; set; }

    // Navigation properties
    public VideoAnalytics? VideoAnalytics { get; set; }
}