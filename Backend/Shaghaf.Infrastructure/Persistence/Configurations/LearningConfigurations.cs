using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Learning;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");
        builder.Property(e => e.Progress).HasPrecision(5, 2);

        builder.HasOne(e => e.Student)
            .WithMany(s => s!.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Course)
            .WithMany(c => c!.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(e => e.CourseId);
    }
}

public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(EntityTypeBuilder<LessonProgress> builder)
    {
        builder.ToTable("LessonProgresses");
        builder.Property(p => p.Status).HasConversion<int>();

        builder.HasOne(p => p.Student)
            .WithMany(s => s!.LessonProgresses)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Lesson)
            .WithMany(l => l!.LessonProgresses)
            .HasForeignKey(p => p.LessonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Course)
            .WithMany()
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.StudentId, p.LessonId }).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(p => new { p.StudentId, p.CourseId, p.LastAccessedAt });
    }
}

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.ToTable("Achievements");
        builder.Property(a => a.Name).IsRequired().HasMaxLength(150);
        builder.Property(a => a.Description).HasMaxLength(1000);
        builder.Property(a => a.IconUrl).HasMaxLength(512);
        builder.Property(a => a.Type).HasConversion<int>();
        builder.Property(a => a.Rarity).HasConversion<int>();

        builder.HasIndex(a => a.Name).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}

public class StudentAchievementConfiguration : IEntityTypeConfiguration<StudentAchievement>
{
    public void Configure(EntityTypeBuilder<StudentAchievement> builder)
    {
        builder.ToTable("StudentAchievements");

        builder.HasOne(sa => sa.Student)
            .WithMany(s => s!.Achievements)
            .HasForeignKey(sa => sa.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sa => sa.Achievement)
            .WithMany(a => a!.StudentAchievements)
            .HasForeignKey(sa => sa.AchievementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sa => new { sa.StudentId, sa.AchievementId }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
