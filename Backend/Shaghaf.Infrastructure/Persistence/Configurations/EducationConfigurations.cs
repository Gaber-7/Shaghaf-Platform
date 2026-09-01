using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Education;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class EducationStageConfiguration : IEntityTypeConfiguration<EducationStage>
{
    public void Configure(EntityTypeBuilder<EducationStage> builder)
    {
        builder.ToTable("EducationStages");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.Type).HasConversion<int>();

        builder.HasIndex(s => s.Type).IsUnique();
    }
}

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.Property(g => g.Description).HasMaxLength(500);

        builder.HasOne(g => g.Stage)
            .WithMany(s => s!.Grades)
            .HasForeignKey(g => g.StageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => g.GradeNumber).IsUnique();
    }
}

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("Subjects");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.IconUrl).HasMaxLength(512);

        builder.HasOne(s => s.Stage)
            .WithMany(st => st!.Subjects)
            .HasForeignKey(s => s.StageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.StageId, s.Name }).IsUnique();
    }
}
