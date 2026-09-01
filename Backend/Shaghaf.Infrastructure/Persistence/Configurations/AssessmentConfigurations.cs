using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shaghaf.Domain.Entities.Assessment;

namespace Shaghaf.Infrastructure.Persistence.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");
        builder.Property(q => q.Title).IsRequired().HasMaxLength(250);
        builder.Property(q => q.Description).HasMaxLength(2000);

        builder.HasOne(q => q.Course)
            .WithMany()
            .HasForeignKey(q => q.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Lesson)
            .WithMany()
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(q => new { q.CourseId, q.IsPublished });
    }
}

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");
        builder.Property(q => q.Content).IsRequired().HasMaxLength(4000);
        builder.Property(q => q.ImageUrl).HasMaxLength(512);
        builder.Property(q => q.ExplanationText).HasMaxLength(4000);
        builder.Property(q => q.Type).HasConversion<int>();
        builder.Property(q => q.DifficultyLevel).HasConversion<int>();

        builder.HasOne(q => q.Quiz)
            .WithMany(z => z!.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => new { q.QuizId, q.Order });
        builder.HasIndex(q => q.QuestionBankId);
    }
}

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");
        builder.Property(a => a.Content).IsRequired().HasMaxLength(2000);
        builder.Property(a => a.ImageUrl).HasMaxLength(512);

        builder.HasOne(a => a.Question)
            .WithMany(q => q!.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.QuestionId, a.Order });
    }
}

public class StudentQuizAttemptConfiguration : IEntityTypeConfiguration<StudentQuizAttempt>
{
    public void Configure(EntityTypeBuilder<StudentQuizAttempt> builder)
    {
        builder.ToTable("StudentQuizAttempts");
        builder.Property(a => a.ScorePercentage).HasPrecision(5, 2);

        builder.HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Quiz)
            .WithMany(q => q!.Attempts)
            .HasForeignKey(a => a.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.StudentId, a.QuizId, a.AttemptNumber }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}

public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
{
    public void Configure(EntityTypeBuilder<StudentAnswer> builder)
    {
        builder.ToTable("StudentAnswers");
        builder.Property(a => a.TextAnswer).HasMaxLength(4000);

        builder.HasOne(a => a.Attempt)
            .WithMany(t => t!.Answers)
            .HasForeignKey(a => a.AttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Question)
            .WithMany(q => q!.StudentAnswers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Answer)
            .WithMany(x => x!.StudentAnswers)
            .HasForeignKey(a => a.AnswerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.AttemptId, a.QuestionId });
    }
}

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments");
        builder.Property(a => a.Title).IsRequired().HasMaxLength(250);
        builder.Property(a => a.Description).HasMaxLength(4000);
        builder.Property(a => a.RubricJson).HasMaxLength(4000);

        builder.HasOne(a => a.Course)
            .WithMany()
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Lesson)
            .WithMany()
            .HasForeignKey(a => a.LessonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.CourseId, a.DueDate });
    }
}

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.ToTable("Submissions");
        builder.Property(s => s.Feedback).HasMaxLength(4000);

        builder.HasOne(s => s.Assignment)
            .WithMany(a => a!.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.GradedByTeacher)
            .WithMany()
            .HasForeignKey(s => s.GradedByTeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.AssignmentId, s.StudentId }).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}

public class SubmissionFileConfiguration : IEntityTypeConfiguration<SubmissionFile>
{
    public void Configure(EntityTypeBuilder<SubmissionFile> builder)
    {
        builder.ToTable("SubmissionFiles");
        builder.Property(f => f.FileName).IsRequired().HasMaxLength(250);
        builder.Property(f => f.FileUrl).IsRequired().HasMaxLength(1024);

        builder.HasOne(f => f.Submission)
            .WithMany(s => s!.Files)
            .HasForeignKey(f => f.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
