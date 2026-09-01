using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.HasDiscriminator<string>("UserType")
            .HasValue<User>(nameof(User))
            .HasValue<Student>(nameof(Student))
            .HasValue<Teacher>(nameof(Teacher))
            .HasValue<Parent>(nameof(Parent))
            .HasValue<Admin>(nameof(Admin));

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PhoneNumber).HasMaxLength(32);
        builder.Property(u => u.ProfilePictureUrl).HasMaxLength(512);
        builder.Property(u => u.RefreshToken).HasMaxLength(512);
        builder.Property(u => u.Role).HasConversion<int>();

        builder.HasIndex(u => u.Email).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(u => u.Role);
        builder.HasIndex(u => u.IsDeleted);
    }
}

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasOne(s => s.Parent)
            .WithMany(p => p!.Students)
            .HasForeignKey(s => s.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.GradeId);
        builder.HasIndex(s => s.ParentId);
    }
}

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.Property(t => t.Biography).HasMaxLength(4000);
        builder.Property(t => t.Qualifications).HasMaxLength(1000);
        builder.Property(t => t.Rating).HasPrecision(3, 2);

        // Replies are authored through the generic User author navigation.
        builder.Ignore(t => t.Replies);

        builder.HasIndex(t => t.IsFeatured);
    }
}

public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.Property(p => p.Occupation).HasMaxLength(150);
        builder.Property(p => p.Relationship).HasMaxLength(50);

        // Preferences are linked through the generic User navigation.
        builder.Ignore(p => p.NotificationPreferences);
    }
}

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.Property(a => a.Department).HasMaxLength(150);

        builder.Property(a => a.Permissions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>(),
                new ValueComparer<List<string>>(
                    (a, b) => a != null && b != null && a.SequenceEqual(b),
                    v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
                    v => v.ToList()))
            .HasMaxLength(2000);
    }
}
