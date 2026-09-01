using Shaghaf.Domain.Entities.Learning;

namespace Shaghaf.Application.Features.Learning.Dtos;

public record EnrollmentDto(
    Guid Id,
    Guid CourseId,
    string CourseTitle,
    string? ThumbnailUrl,
    DateTime EnrolledAt,
    DateTime? CompletedAt,
    decimal Progress,
    int TotalLessonCount,
    int CompletedLessonCount);

public record LessonProgressDto(
    Guid LessonId,
    Guid CourseId,
    ProgressStatus Status,
    int TimeSpentSeconds,
    DateTime StartedAt,
    DateTime? CompletedAt,
    DateTime LastAccessedAt);

public record TrackLessonProgressRequest(int TimeSpentSeconds, bool Completed);

public record CourseProgressDto(
    EnrollmentDto Enrollment,
    IReadOnlyList<LessonProgressDto> Lessons);
