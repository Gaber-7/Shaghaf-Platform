using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Media;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("Videos");
        builder.Property(v => v.Title).IsRequired().HasMaxLength(250);
        builder.Property(v => v.Description).HasMaxLength(4000);
        builder.Property(v => v.Url).IsRequired().HasMaxLength(1024);
        builder.Property(v => v.Provider).IsRequired().HasMaxLength(50);
        builder.Property(v => v.ThumbnailUrl).HasMaxLength(512);

        builder.HasOne(v => v.Lesson)
            .WithOne(l => l!.Video)
            .HasForeignKey<Video>(v => v.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.LessonId).IsUnique();
    }
}

public class VideoWatchSessionConfiguration : IEntityTypeConfiguration<VideoWatchSession>
{
    public void Configure(EntityTypeBuilder<VideoWatchSession> builder)
    {
        builder.ToTable("VideoWatchSessions");
        builder.Property(s => s.WatchPercentage).HasPrecision(5, 2);

        builder.HasOne(s => s.Video)
            .WithMany(v => v!.WatchSessions)
            .HasForeignKey(s => s.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Supports resume-playback lookups and per-video aggregation jobs.
        builder.HasIndex(s => new { s.VideoId, s.StudentId, s.StartedAt });
        builder.HasIndex(s => s.StartedAt);
    }
}

public class VideoAnalyticsConfiguration : IEntityTypeConfiguration<VideoAnalytics>
{
    public void Configure(EntityTypeBuilder<VideoAnalytics> builder)
    {
        builder.ToTable("VideoAnalytics");
        builder.Property(a => a.AverageWatchDurationSeconds).HasPrecision(10, 2);
        builder.Property(a => a.AverageWatchPercentage).HasPrecision(5, 2);
        builder.Property(a => a.CompletionRate).HasPrecision(5, 2);

        builder.HasOne(a => a.Video)
            .WithOne(v => v!.Analytics)
            .HasForeignKey<VideoAnalytics>(a => a.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.VideoId).IsUnique();
    }
}

public class VideoAnalyticsDailySnapshotConfiguration : IEntityTypeConfiguration<VideoAnalyticsDailySnapshot>
{
    public void Configure(EntityTypeBuilder<VideoAnalyticsDailySnapshot> builder)
    {
        builder.ToTable("VideoAnalyticsDailySnapshots");
        builder.Property(s => s.CompletionRate).HasPrecision(5, 2);

        builder.HasOne(s => s.VideoAnalytics)
            .WithMany(a => a!.DailySnapshots)
            .HasForeignKey(s => s.VideoAnalyticsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.VideoAnalyticsId, s.Date }).IsUnique();
    }
}
