using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Certificates;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("Certificates");
        builder.Property(c => c.CertificateNumber).IsRequired().HasMaxLength(50);
        builder.Property(c => c.VerificationCode).IsRequired().HasMaxLength(64);
        builder.Property(c => c.FileUrl).HasMaxLength(1024);
        builder.Property(c => c.Status).HasConversion<int>();

        builder.HasOne(c => c.Student)
            .WithMany()
            .HasForeignKey(c => c.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Course)
            .WithMany()
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.CertificateNumber).IsUnique();
        builder.HasIndex(c => c.VerificationCode).IsUnique();
        builder.HasIndex(c => new { c.StudentId, c.CourseId });
    }
}
