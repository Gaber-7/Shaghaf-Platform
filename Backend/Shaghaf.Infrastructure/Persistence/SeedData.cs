using Microsoft.EntityFrameworkCore;
using Shaghaf.Domain.Entities.Education;
using Shaghaf.Domain.Entities.Engagement;
using Shaghaf.Domain.Entities.Payments;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Infrastructure.Persistence;

/// <summary>
/// Reference data shipped with the schema: the educational hierarchy,
/// the engagement scoring weights and the default subscription plans.
/// </summary>
internal static class SeedData
{
    private static readonly DateTime SeededAt = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Apply(ModelBuilder modelBuilder)
    {
        SeedStages(modelBuilder);
        SeedGrades(modelBuilder);
        SeedSubjects(modelBuilder);
        SeedEngagementWeights(modelBuilder);
        SeedSubscriptionPlans(modelBuilder);
    }

    private static void SeedStages(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EducationStage>().HasData(
            Stage(1, "Primary", EducationStageType.Primary, 1),
            Stage(2, "Preparatory", EducationStageType.Preparatory, 2),
            Stage(3, "Secondary", EducationStageType.Secondary, 3));
    }

    private static void SeedGrades(ModelBuilder modelBuilder)
    {
        var grades = new List<Grade>();
        for (var gradeNumber = 3; gradeNumber <= 12; gradeNumber++)
        {
            var stageId = gradeNumber switch
            {
                <= 6 => 1,
                <= 9 => 2,
                _ => 3
            };

            grades.Add(new Grade
            {
                Id = gradeNumber,
                StageId = stageId,
                Name = $"Grade {gradeNumber}",
                GradeNumber = gradeNumber,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        modelBuilder.Entity<Grade>().HasData(grades);
    }

    private static void SeedSubjects(ModelBuilder modelBuilder)
    {
        var subjectsByStage = new Dictionary<int, string[]>
        {
            [1] = ["Arabic", "English", "Mathematics", "Science", "Social Studies", "Religious Education"],
            [2] = ["Arabic", "English", "Mathematics", "Science", "Social Studies", "Religious Education"],
            [3] = ["Arabic", "English", "Mathematics", "Physics", "Chemistry", "Biology", "History", "Geography", "Philosophy"]
        };

        var subjects = new List<Subject>();
        var id = 1;
        foreach (var (stageId, names) in subjectsByStage)
        {
            var order = 1;
            foreach (var name in names)
            {
                subjects.Add(new Subject
                {
                    Id = id++,
                    StageId = stageId,
                    Name = name,
                    Order = order++,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt
                });
            }
        }

        modelBuilder.Entity<Subject>().HasData(subjects);
    }

    private static void SeedEngagementWeights(ModelBuilder modelBuilder)
    {
        var weights = new (string Component, decimal Weight)[]
        {
            ("VideoEngagement", 0.25m),
            ("StudentInteraction", 0.20m),
            ("QnaActivity", 0.15m),
            ("LiveClassActivity", 0.15m),
            ("CourseCompletion", 0.15m),
            ("Rating", 0.10m)
        };

        modelBuilder.Entity<EngagementWeightConfig>().HasData(
            weights.Select((w, index) => new EngagementWeightConfig
            {
                Id = index + 1,
                Component = w.Component,
                Weight = w.Weight,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            }));
    }

    private static void SeedSubscriptionPlans(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionPlan>().HasData(
            Plan(1, "Basic Monthly", SubscriptionTier.Basic, SubscriptionType.Monthly, 199m, 30),
            Plan(2, "Premium Monthly", SubscriptionTier.Premium, SubscriptionType.Monthly, 349m, 30),
            Plan(3, "Premium Plus Yearly", SubscriptionTier.PremiumPlus, SubscriptionType.Yearly, 2999m, 365));
    }

    private static EducationStage Stage(int id, string name, EducationStageType type, int order) => new()
    {
        Id = id,
        Name = name,
        Type = type,
        Order = order,
        CreatedAt = SeededAt,
        UpdatedAt = SeededAt
    };

    private static SubscriptionPlan Plan(int id, string name, SubscriptionTier tier, SubscriptionType type, decimal price, int durationDays) => new()
    {
        Id = id,
        Name = name,
        Tier = tier,
        Type = type,
        Price = price,
        DurationDays = durationDays,
        CreatedAt = SeededAt,
        UpdatedAt = SeededAt
    };
}
