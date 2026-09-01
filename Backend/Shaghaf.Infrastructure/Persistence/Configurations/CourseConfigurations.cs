using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Courses;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.Property(c => c.Title).IsRequired().HasMaxLength(250);
        builder.Property(c => c.Description).HasMaxLength(4000);
        builder.Property(c => c.ThumbnailUrl).HasMaxLength(512);
        builder.Property(c => c.Price).HasPrecision(10, 2);
        builder.Property(c => c.Rating).HasPrecision(3, 2);
        builder.Property(c => c.DifficultyLevel).HasConversion<int>();

        builder.HasOne(c => c.Teacher)
            .WithMany(t => t!.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Grade)
            .WithMany(g => g!.Courses)
            .HasForeignKey(c => c.GradeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Subject)
            .WithMany(s => s!.Courses)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.GradeId, c.SubjectId, c.IsPublished });
        builder.HasIndex(c => c.TeacherId);
        builder.HasIndex(c => c.Rating);
    }
}

public class CourseSectionConfiguration : IEntityTypeConfiguration<CourseSection>
{
    public void Configure(EntityTypeBuilder<CourseSection> builder)
    {
        builder.ToTable("CourseSections");
        builder.Property(s => s.Title).IsRequired().HasMaxLength(250);
        builder.Property(s => s.Description).HasMaxLength(2000);

        builder.HasOne(s => s.Course)
            .WithMany(c => c!.Sections)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.CourseId, s.Order });
    }
}

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons");
        builder.Property(l => l.Title).IsRequired().HasMaxLength(250);
        builder.Property(l => l.Description).HasMaxLength(4000);
        builder.Property(l => l.Type).HasConversion<int>();

        builder.HasOne(l => l.Section)
            .WithMany(s => s!.Lessons)
            .HasForeignKey(l => l.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Course)
            .WithMany()
            .HasForeignKey(l => l.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => new { l.CourseId, l.Order });
        builder.HasIndex(l => l.SectionId);
    }
}
