using Shaghaf.Application.Common.Exceptions;
using Shaghaf.Application.Common.Interfaces;
using Shaghaf.Application.Common.Models;
using Shaghaf.Application.Features.Learning.Dtos;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Learning;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Application.Features.Learning;

public class EnrollmentService : IEnrollmentService
{
    private const int MaxPageSize = 100;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueryExecutor _queryExecutor;
    private readonly ICurrentUserService _currentUser;

    public EnrollmentService(IUnitOfWork unitOfWork, IQueryExecutor queryExecutor, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _queryExecutor = queryExecutor;
        _currentUser = currentUser;
    }

    public async Task<EnrollmentDto> EnrollAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var studentId = RequireStudentId();

        var course = await _unitOfWork.Repository<Course>()
            .FirstOrDefaultAsync(c => c.Id == courseId, asTracking: true, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), courseId);

        if (!course.IsPublished)
        {
            throw new ConflictException("This course is not published yet.");
        }

        if (!course.IsFree)
        {
            // Paid enrollment goes through the subscription/payment flow (later phase).
            throw new ConflictException("This course requires an active subscription or purchase.");
        }

        if (await _unitOfWork.Repository<Enrollment>().ExistsAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken))
        {
            throw new ConflictException("You are already enrolled in this course.");
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow,
            TotalLessonCount = course.LessonCount
        };

        await _unitOfWork.Repository<Enrollment>().AddAsync(enrollment, cancellationToken);
        course.StudentCount += 1;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetEnrollmentDtoAsync(enrollment.Id, cancellationToken);
    }

    public Task<PagedResult<EnrollmentDto>> GetMyEnrollmentsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var studentId = RequireStudentId();

        var query = _unitOfWork.Repository<Enrollment>().Query()
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.EnrolledAt)
            .Select(ToDto);

        return _queryExecutor.ToPagedResultAsync(query, Math.Max(page, 1), Math.Clamp(pageSize, 1, MaxPageSize), cancellationToken);
    }

    public async Task<CourseProgressDto> GetCourseProgressAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var studentId = RequireStudentId();

        var enrollment = await _queryExecutor.FirstOrDefaultAsync(
            _unitOfWork.Repository<Enrollment>().Query()
                .Where(e => e.StudentId == studentId && e.CourseId == courseId)
                .Select(ToDto),
            cancellationToken) ?? throw new NotFoundException(nameof(Enrollment), courseId);

        var lessons = await _queryExecutor.ToListAsync(
            _unitOfWork.Repository<LessonProgress>().Query()
                .Where(p => p.StudentId == studentId && p.CourseId == courseId)
                .OrderBy(p => p.StartedAt)
                .Select(p => new LessonProgressDto(
                    p.LessonId, p.CourseId, p.Status, p.TimeSpentSeconds, p.StartedAt, p.CompletedAt, p.LastAccessedAt)),
            cancellationToken);

        return new CourseProgressDto(enrollment, lessons);
    }

    public async Task<LessonProgressDto> TrackLessonAsync(Guid lessonId, TrackLessonProgressRequest request, CancellationToken cancellationToken = default)
    {
        var studentId = RequireStudentId();

        var lesson = await _unitOfWork.Repository<Lesson>()
            .FirstOrDefaultAsync(l => l.Id == lessonId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Lesson), lessonId);

        var enrollment = await _unitOfWork.Repository<Enrollment>()
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == lesson.CourseId, asTracking: true, cancellationToken)
            ?? throw new ForbiddenException("You are not enrolled in this course.");

        var now = DateTime.UtcNow;
        var progress = await _unitOfWork.Repository<LessonProgress>()
            .FirstOrDefaultAsync(p => p.StudentId == studentId && p.LessonId == lessonId, asTracking: true, cancellationToken);

        if (progress is null)
        {
            progress = new LessonProgress
            {
                StudentId = studentId,
                LessonId = lessonId,
                CourseId = lesson.CourseId,
                StartedAt = now,
                Status = ProgressStatus.InProgress
            };

            await _unitOfWork.Repository<LessonProgress>().AddAsync(progress, cancellationToken);
        }

        progress.TimeSpentSeconds += Math.Max(request.TimeSpentSeconds, 0);
        progress.LastAccessedAt = now;

        if (request.Completed && progress.Status != ProgressStatus.Completed)
        {
            progress.Status = ProgressStatus.Completed;
            progress.CompletedAt = now;
            enrollment.CompletedLessonCount += 1;
        }
        else if (progress.Status == ProgressStatus.NotStarted)
        {
            progress.Status = ProgressStatus.InProgress;
        }

        RecalculateEnrollment(enrollment, lesson.CourseId, now);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LessonProgressDto(
            progress.LessonId, progress.CourseId, progress.Status, progress.TimeSpentSeconds,
            progress.StartedAt, progress.CompletedAt, progress.LastAccessedAt);
    }

    private static void RecalculateEnrollment(Enrollment enrollment, Guid courseId, DateTime now)
    {
        if (enrollment.TotalLessonCount <= 0)
        {
            enrollment.Progress = 0;

            return;
        }

        enrollment.CompletedLessonCount = Math.Min(enrollment.CompletedLessonCount, enrollment.TotalLessonCount);
        enrollment.Progress = Math.Round(enrollment.CompletedLessonCount * 100m / enrollment.TotalLessonCount, 2);
        enrollment.CompletedAt = enrollment.Progress >= 100m ? enrollment.CompletedAt ?? now : null;
    }

    private async Task<EnrollmentDto> GetEnrollmentDtoAsync(Guid enrollmentId, CancellationToken cancellationToken) =>
        await _queryExecutor.FirstOrDefaultAsync(
            _unitOfWork.Repository<Enrollment>().Query().Where(e => e.Id == enrollmentId).Select(ToDto), cancellationToken)
        ?? throw new NotFoundException(nameof(Enrollment), enrollmentId);

    private Guid RequireStudentId() => _currentUser.Role == UserRole.Student && _currentUser.UserId is { } id
        ? id
        : throw new ForbiddenException("Only students can access enrollments.");

    private static System.Linq.Expressions.Expression<Func<Enrollment, EnrollmentDto>> ToDto => enrollment =>
        new EnrollmentDto(
            enrollment.Id,
            enrollment.CourseId,
            enrollment.Course != null ? enrollment.Course.Title : string.Empty,
            enrollment.Course != null ? enrollment.Course.ThumbnailUrl : null,
            enrollment.EnrolledAt,
            enrollment.CompletedAt,
            enrollment.Progress,
            enrollment.TotalLessonCount,
            enrollment.CompletedLessonCount);
}
