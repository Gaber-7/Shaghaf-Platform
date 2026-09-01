using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Interaction;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class LessonQuestionConfiguration : IEntityTypeConfiguration<LessonQuestion>
{
    public void Configure(EntityTypeBuilder<LessonQuestion> builder)
    {
        builder.ToTable("LessonQuestions");
        builder.Property(q => q.Title).IsRequired().HasMaxLength(250);
        builder.Property(q => q.Content).IsRequired().HasMaxLength(4000);

        builder.HasOne(q => q.Lesson)
            .WithMany(l => l!.Questions)
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Student)
            .WithMany()
            .HasForeignKey(q => q.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(q => new { q.LessonId, q.IsResolved });
    }
}

public class LessonReplyConfiguration : IEntityTypeConfiguration<LessonReply>
{
    public void Configure(EntityTypeBuilder<LessonReply> builder)
    {
        builder.ToTable("LessonReplies");
        builder.Property(r => r.Content).IsRequired().HasMaxLength(4000);

        builder.HasOne(r => r.Question)
            .WithMany(q => q!.Replies)
            .HasForeignKey(r => r.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Author)
            .WithMany()
            .HasForeignKey(r => r.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.QuestionId, r.IsAcceptedAnswer });
        builder.HasIndex(r => r.AuthorId);
    }
}

public class LiveClassConfiguration : IEntityTypeConfiguration<LiveClass>
{
    public void Configure(EntityTypeBuilder<LiveClass> builder)
    {
        builder.ToTable("LiveClasses");
        builder.Property(c => c.Title).IsRequired().HasMaxLength(250);
        builder.Property(c => c.RecordingUrl).HasMaxLength(1024);
        builder.Property(c => c.Status).HasConversion<int>();

        builder.HasOne(c => c.Teacher)
            .WithMany(t => t!.LiveClasses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Course)
            .WithMany()
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.CourseId, c.ScheduledStartTime });
        builder.HasIndex(c => new { c.TeacherId, c.ScheduledStartTime });
    }
}

public class ClassAttendanceConfiguration : IEntityTypeConfiguration<ClassAttendance>
{
    public void Configure(EntityTypeBuilder<ClassAttendance> builder)
    {
        builder.ToTable("ClassAttendances");
        builder.Property(a => a.ParticipationLevel).HasConversion<int>();

        builder.HasOne(a => a.Class)
            .WithMany(c => c!.Attendance)
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.ClassId, a.StudentId }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
