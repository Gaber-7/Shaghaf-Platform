using Shaghaf.Domain.Enums;

namespace Shaghaf.Application.Features.Courses.Dtos;

public record CourseQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public int? GradeId { get; init; }
    public int? SubjectId { get; init; }
    public Guid? TeacherId { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }
    public bool? IsFree { get; init; }
    public CourseSort Sort { get; init; } = CourseSort.Newest;
}

public enum CourseSort
{
    Newest = 1,
    Rating = 2,
    Popularity = 3,
    PriceAscending = 4
}

public record CourseListItemDto(
    Guid Id,
    string Title,
    string? Description,
    string? ThumbnailUrl,
    Guid TeacherId,
    string TeacherName,
    int GradeId,
    int SubjectId,
    DifficultyLevel DifficultyLevel,
    decimal Price,
    bool IsFree,
    bool IsPublished,
    decimal Rating,
    int StudentCount,
    int LessonCount,
    int TotalDurationSeconds);

public record LessonDto(
    Guid Id,
    Guid SectionId,
    string Title,
    string? Description,
    int Order,
    LessonType Type,
    bool IsPreview,
    bool IsPublished);

public record SectionDto(
    Guid Id,
    string Title,
    string? Description,
    int Order,
    IReadOnlyList<LessonDto> Lessons);

public record CourseDetailDto(
    CourseListItemDto Course,
    IReadOnlyList<SectionDto> Sections);

public record CreateCourseRequest(
    string Title,
    string? Description,
    string? ThumbnailUrl,
    int GradeId,
    int SubjectId,
    DifficultyLevel DifficultyLevel,
    decimal Price,
    bool IsFree);

public record UpdateCourseRequest(
    string Title,
    string? Description,
    string? ThumbnailUrl,
    DifficultyLevel DifficultyLevel,
    decimal Price,
    bool IsFree);

public record CreateSectionRequest(string Title, string? Description, int Order);

public record UpdateSectionRequest(string Title, string? Description, int Order);

public record CreateLessonRequest(
    Guid SectionId,
    string Title,
    string? Description,
    int Order,
    LessonType Type,
    bool IsPreview);

public record UpdateLessonRequest(
    string Title,
    string? Description,
    int Order,
    LessonType Type,
    bool IsPreview,
    bool IsPublished);
