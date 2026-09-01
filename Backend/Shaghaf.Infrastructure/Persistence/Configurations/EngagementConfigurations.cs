using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Engagement;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class TeacherEngagementScoreConfiguration : IEntityTypeConfiguration<TeacherEngagementScore>
{
    public void Configure(EntityTypeBuilder<TeacherEngagementScore> builder)
    {
        builder.ToTable("TeacherEngagementScores");
        builder.Property(s => s.Period).HasConversion<int>();

        foreach (var property in new[]
                 {
                     nameof(TeacherEngagementScore.TotalScore),
                     nameof(TeacherEngagementScore.VideoEngagementScore),
                     nameof(TeacherEngagementScore.StudentInteractionScore),
                     nameof(TeacherEngagementScore.QnaActivityScore),
                     nameof(TeacherEngagementScore.LiveClassActivityScore),
                     nameof(TeacherEngagementScore.CourseCompletionScore),
                     nameof(TeacherEngagementScore.RatingScore),
                     nameof(TeacherEngagementScore.StudentRetentionScore)
                 })
        {
            builder.Property<decimal>(property).HasPrecision(6, 2);
        }

        builder.HasOne(s => s.Teacher)
            .WithMany(t => t!.EngagementScores)
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.TeacherId, s.Period, s.PeriodStart }).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(s => new { s.Period, s.TotalScore });
    }
}

public class EngagementWeightConfigConfiguration : IEntityTypeConfiguration<EngagementWeightConfig>
{
    public void Configure(EntityTypeBuilder<EngagementWeightConfig> builder)
    {
        builder.ToTable("EngagementWeightConfigs");
        builder.Property(c => c.Component).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Weight).HasPrecision(5, 4);

        builder.HasIndex(c => c.Component).IsUnique();
    }
}
