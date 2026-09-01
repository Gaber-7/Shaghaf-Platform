using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Shaghaf.Domain.Entities.Assessment;
using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Certificates;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Education;
using Shaghaf.Domain.Entities.Engagement;
using Shaghaf.Domain.Entities.Interaction;
using Shaghaf.Domain.Entities.Learning;
using Shaghaf.Domain.Entities.Media;
using Shaghaf.Domain.Entities.Notifications;
using Shaghaf.Domain.Entities.Payments;
using Shaghaf.Domain.Entities.Reviews;
using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Infrastructure.Persistence;

public class ShaghafDbContext : DbContext
{
    public ShaghafDbContext(DbContextOptions<ShaghafDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<Admin> Admins => Set<Admin>();

    public DbSet<EducationStage> EducationStages => Set<EducationStage>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Subject> Subjects => Set<Subject>();

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseSection> CourseSections => Set<CourseSection>();
    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<Video> Videos => Set<Video>();
    public DbSet<VideoWatchSession> VideoWatchSessions => Set<VideoWatchSession>();
    public DbSet<VideoAnalytics> VideoAnalytics => Set<VideoAnalytics>();
    public DbSet<VideoAnalyticsDailySnapshot> VideoAnalyticsDailySnapshots => Set<VideoAnalyticsDailySnapshot>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<StudentAchievement> StudentAchievements => Set<StudentAchievement>();

    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<StudentQuizAttempt> StudentQuizAttempts => Set<StudentQuizAttempt>();
    public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<SubmissionFile> SubmissionFiles => Set<SubmissionFile>();

    public DbSet<LessonQuestion> LessonQuestions => Set<LessonQuestion>();
    public DbSet<LessonReply> LessonReplies => Set<LessonReply>();
    public DbSet<LiveClass> LiveClasses => Set<LiveClass>();
    public DbSet<ClassAttendance> ClassAttendances => Set<ClassAttendance>();

    public DbSet<CourseReview> CourseReviews => Set<CourseReview>();
    public DbSet<TeacherReview> TeacherReviews => Set<TeacherReview>();

    public DbSet<TeacherEngagementScore> TeacherEngagementScores => Set<TeacherEngagementScore>();
    public DbSet<EngagementWeightConfig> EngagementWeightConfigs => Set<EngagementWeightConfig>();

    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();

    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Coupon> Coupons => Set<Coupon>();

    public DbSet<Certificate> Certificates => Set<Certificate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ApplySoftDeleteQueryFilters(modelBuilder);
        SeedData.Apply(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyAuditAndSoftDelete();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditAndSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.BaseType is not null)
            {
                continue;
            }

            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) &&
                !typeof(BaseEntityInt).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var body = Expression.Not(Expression.Property(parameter, nameof(BaseEntity.IsDeleted)));
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
        }
    }

    private void ApplyAuditAndSoftDelete()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    SetProperty(entry, nameof(BaseEntity.CreatedAt), now);
                    SetProperty(entry, nameof(BaseEntity.UpdatedAt), now);
                    break;
                case EntityState.Modified:
                    SetProperty(entry, nameof(BaseEntity.UpdatedAt), now);
                    break;
                case EntityState.Deleted:
                    if (entry.Metadata.FindProperty(nameof(BaseEntity.IsDeleted)) is null)
                    {
                        break;
                    }

                    entry.State = EntityState.Modified;
                    SetProperty(entry, nameof(BaseEntity.IsDeleted), true);
                    SetProperty(entry, nameof(BaseEntity.DeletedAt), now);
                    SetProperty(entry, nameof(BaseEntity.UpdatedAt), now);
                    break;
            }
        }
    }

    private static void SetProperty(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, string propertyName, object value)
    {
        if (entry.Metadata.FindProperty(propertyName) is not null)
        {
            entry.Property(propertyName).CurrentValue = value;
        }
    }
}
