using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Reviews;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class CourseReviewConfiguration : IEntityTypeConfiguration<CourseReview>
{
    public void Configure(EntityTypeBuilder<CourseReview> builder)
    {
        builder.ToTable("CourseReviews");
        builder.Property(r => r.Content).HasMaxLength(4000);

        builder.HasOne(r => r.Course)
            .WithMany(c => c!.Reviews)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // One review per student per course keeps course ratings unmanipulable.
        builder.HasIndex(r => new { r.CourseId, r.StudentId }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}

public class TeacherReviewConfiguration : IEntityTypeConfiguration<TeacherReview>
{
    public void Configure(EntityTypeBuilder<TeacherReview> builder)
    {
        builder.ToTable("TeacherReviews");
        builder.Property(r => r.Content).HasMaxLength(4000);

        builder.HasOne(r => r.Teacher)
            .WithMany(t => t!.Reviews)
            .HasForeignKey(r => r.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.TeacherId, r.StudentId }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
