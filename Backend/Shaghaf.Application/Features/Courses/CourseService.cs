using Shaghaf.Application.Common.Exceptions;
using Shaghaf.Application.Common.Interfaces;
using Shaghaf.Application.Common.Models;
using Shaghaf.Application.Features.Courses.Dtos;
using Shaghaf.Domain.Entities.Courses;
using Shaghaf.Domain.Entities.Education;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Application.Features.Courses;

public class CourseService : ICourseService
{
    private const int MaxPageSize = 100;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueryExecutor _queryExecutor;
    private readonly ICurrentUserService _currentUser;

    public CourseService(IUnitOfWork unitOfWork, IQueryExecutor queryExecutor, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _queryExecutor = queryExecutor;
        _currentUser = currentUser;
    }

    private bool IsAdmin => _currentUser.Role is UserRole.Admin or UserRole.SuperAdmin;

    public Task<PagedResult<CourseListItemDto>> SearchAsync(CourseQuery query, CancellationToken cancellationToken = default)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var courses = _unitOfWork.Repository<Course>().Query();

        if (!IsAdmin)
        {
            var teacherId = _currentUser.Role == UserRole.Teacher ? _currentUser.UserId : null;
            courses = courses.Where(c => c.IsPublished || (teacherId != null && c.TeacherId == teacherId));
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            courses = courses.Where(c => c.Title.Contains(search) || (c.Description != null && c.Description.Contains(search)));
        }

        if (query.GradeId is { } gradeId)
        {
            courses = courses.Where(c => c.GradeId == gradeId);
        }

        if (query.SubjectId is { } subjectId)
        {
            courses = courses.Where(c => c.SubjectId == subjectId);
        }

        if (query.TeacherId is { } filterTeacherId)
        {
            courses = courses.Where(c => c.TeacherId == filterTeacherId);
        }

        if (query.DifficultyLevel is { } difficulty)
        {
            courses = courses.Where(c => c.DifficultyLevel == difficulty);
        }

        if (query.IsFree is { } isFree)
        {
            courses = courses.Where(c => c.IsFree == isFree);
        }

        courses = query.Sort switch
        {
            CourseSort.Rating => courses.OrderByDescending(c => c.Rating).ThenByDescending(c => c.StudentCount),
            CourseSort.Popularity => courses.OrderByDescending(c => c.StudentCount).ThenByDescending(c => c.Rating),
            CourseSort.PriceAscending => courses.OrderBy(c => c.Price).ThenBy(c => c.Title),
            _ => courses.OrderByDescending(c => c.CreatedAt)
        };

        return _queryExecutor.ToPagedResultAsync(courses.Select(ToListItem), page, pageSize, cancellationToken);
    }

    public async Task<CourseDetailDto> GetAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var course = await _queryExecutor.FirstOrDefaultAsync(
            _unitOfWork.Repository<Course>().Query().Where(c => c.Id == courseId).Select(ToListItem),
            cancellationToken) ?? throw new NotFoundException(nameof(Course), courseId);

        var isOwnerOrAdmin = IsAdmin || course.TeacherId == _currentUser.UserId;
        if (!course.IsPublished && !isOwnerOrAdmin)
        {
            throw new NotFoundException(nameof(Course), courseId);
        }

        var sections = await _queryExecutor.ToListAsync(
            _unitOfWork.Repository<CourseSection>().Query()
                .Where(s => s.CourseId == courseId)
                .OrderBy(s => s.Order)
                .Select(s => new SectionDto(
                    s.Id,
                    s.Title,
                    s.Description,
                    s.Order,
                    s.Lessons
                        .Where(l => l.IsPublished || isOwnerOrAdmin)
                        .OrderBy(l => l.Order)
                        .Select(l => new LessonDto(l.Id, l.SectionId, l.Title, l.Description, l.Order, l.Type, l.IsPreview, l.IsPublished))
                        .ToList())),
            cancellationToken);

        return new CourseDetailDto(course, sections);
    }

    public async Task<CourseListItemDto> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
    {
        if (_currentUser.Role != UserRole.Teacher || _currentUser.UserId is not { } teacherId)
        {
            throw new ForbiddenException("Only teachers can create courses.");
        }

        if (!await _unitOfWork.RepositoryInt<Grade>().ExistsAsync(g => g.Id == request.GradeId, cancellationToken))
        {
            throw new NotFoundException(nameof(Grade), request.GradeId);
        }

        if (!await _unitOfWork.RepositoryInt<Subject>().ExistsAsync(s => s.Id == request.SubjectId, cancellationToken))
        {
            throw new NotFoundException(nameof(Subject), request.SubjectId);
        }

        var course = new Course
        {
            TeacherId = teacherId,
            GradeId = request.GradeId,
            SubjectId = request.SubjectId,
            Title = request.Title.Trim(),
            Description = request.Description,
            ThumbnailUrl = request.ThumbnailUrl,
            DifficultyLevel = request.DifficultyLevel,
            Price = request.IsFree ? 0 : request.Price,
            IsFree = request.IsFree
        };

        await _unitOfWork.Repository<Course>().AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetListItemAsync(course.Id, cancellationToken);
    }

    public async Task<CourseListItemDto> UpdateAsync(Guid courseId, UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var course = await GetEditableCourseAsync(courseId, cancellationToken);

        course.Title = request.Title.Trim();
        course.Description = request.Description;
        course.ThumbnailUrl = request.ThumbnailUrl;
        course.DifficultyLevel = request.DifficultyLevel;
        course.IsFree = request.IsFree;
        course.Price = request.IsFree ? 0 : request.Price;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetListItemAsync(courseId, cancellationToken);
    }

    public async Task SetPublishedAsync(Guid courseId, bool isPublished, CancellationToken cancellationToken = default)
    {
        var course = await GetEditableCourseAsync(courseId, cancellationToken);

        if (isPublished && course.LessonCount == 0)
        {
            throw new ConflictException("A course cannot be published without lessons.");
        }

        course.IsPublished = isPublished;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var course = await GetEditableCourseAsync(courseId, cancellationToken);

        if (course.StudentCount > 0)
        {
            throw new ConflictException("A course with enrolled students cannot be deleted; unpublish it instead.");
        }

        _unitOfWork.Repository<Course>().Remove(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<SectionDto> AddSectionAsync(Guid courseId, CreateSectionRequest request, CancellationToken cancellationToken = default)
    {
        await GetEditableCourseAsync(courseId, cancellationToken);

        var section = new CourseSection
        {
            CourseId = courseId,
            Title = request.Title.Trim(),
            Description = request.Description,
            Order = request.Order
        };

        await _unitOfWork.Repository<CourseSection>().AddAsync(section, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SectionDto(section.Id, section.Title, section.Description, section.Order, []);
    }

    public async Task<SectionDto> UpdateSectionAsync(Guid sectionId, UpdateSectionRequest request, CancellationToken cancellationToken = default)
    {
        var section = await GetTrackedSectionAsync(sectionId, cancellationToken);
        await GetEditableCourseAsync(section.CourseId, cancellationToken);

        section.Title = request.Title.Trim();
        section.Description = request.Description;
        section.Order = request.Order;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SectionDto(section.Id, section.Title, section.Description, section.Order, []);
    }

    public async Task DeleteSectionAsync(Guid sectionId, CancellationToken cancellationToken = default)
    {
        var section = await GetTrackedSectionAsync(sectionId, cancellationToken);
        var course = await GetEditableCourseAsync(section.CourseId, cancellationToken);

        var lessonCount = await _queryExecutor.CountAsync(
            _unitOfWork.Repository<Lesson>().Query().Where(l => l.SectionId == sectionId), cancellationToken);

        _unitOfWork.Repository<CourseSection>().Remove(section);
        course.LessonCount -= lessonCount;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<LessonDto> AddLessonAsync(Guid courseId, CreateLessonRequest request, CancellationToken cancellationToken = default)
    {
        var course = await GetEditableCourseAsync(courseId, cancellationToken);

        var sectionBelongsToCourse = await _unitOfWork.Repository<CourseSection>()
            .ExistsAsync(s => s.Id == request.SectionId && s.CourseId == courseId, cancellationToken);

        if (!sectionBelongsToCourse)
        {
            throw new NotFoundException(nameof(CourseSection), request.SectionId);
        }

        var lesson = new Lesson
        {
            CourseId = courseId,
            SectionId = request.SectionId,
            Title = request.Title.Trim(),
            Description = request.Description,
            Order = request.Order,
            Type = request.Type,
            IsPreview = request.IsPreview
        };

        await _unitOfWork.Repository<Lesson>().AddAsync(lesson, cancellationToken);
        course.LessonCount += 1;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToLessonDto(lesson);
    }

    public async Task<LessonDto> UpdateLessonAsync(Guid lessonId, UpdateLessonRequest request, CancellationToken cancellationToken = default)
    {
        var lesson = await GetTrackedLessonAsync(lessonId, cancellationToken);
        await GetEditableCourseAsync(lesson.CourseId, cancellationToken);

        lesson.Title = request.Title.Trim();
        lesson.Description = request.Description;
        lesson.Order = request.Order;
        lesson.Type = request.Type;
        lesson.IsPreview = request.IsPreview;
        lesson.IsPublished = request.IsPublished;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToLessonDto(lesson);
    }

    public async Task DeleteLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var lesson = await GetTrackedLessonAsync(lessonId, cancellationToken);
        var course = await GetEditableCourseAsync(lesson.CourseId, cancellationToken);

        _unitOfWork.Repository<Lesson>().Remove(lesson);
        course.LessonCount = Math.Max(course.LessonCount - 1, 0);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Course> GetEditableCourseAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Repository<Course>()
            .FirstOrDefaultAsync(c => c.Id == courseId, asTracking: true, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), courseId);

        if (!IsAdmin && course.TeacherId != _currentUser.UserId)
        {
            throw new ForbiddenException("Only the owning teacher can modify this course.");
        }

        return course;
    }

    private async Task<CourseSection> GetTrackedSectionAsync(Guid sectionId, CancellationToken cancellationToken) =>
        await _unitOfWork.Repository<CourseSection>()
            .FirstOrDefaultAsync(s => s.Id == sectionId, asTracking: true, cancellationToken)
        ?? throw new NotFoundException(nameof(CourseSection), sectionId);

    private async Task<Lesson> GetTrackedLessonAsync(Guid lessonId, CancellationToken cancellationToken) =>
        await _unitOfWork.Repository<Lesson>()
            .FirstOrDefaultAsync(l => l.Id == lessonId, asTracking: true, cancellationToken)
        ?? throw new NotFoundException(nameof(Lesson), lessonId);

    private async Task<CourseListItemDto> GetListItemAsync(Guid courseId, CancellationToken cancellationToken) =>
        await _queryExecutor.FirstOrDefaultAsync(
            _unitOfWork.Repository<Course>().Query().Where(c => c.Id == courseId).Select(ToListItem), cancellationToken)
        ?? throw new NotFoundException(nameof(Course), courseId);

    private static LessonDto ToLessonDto(Lesson lesson) => new(
        lesson.Id,
        lesson.SectionId,
        lesson.Title,
        lesson.Description,
        lesson.Order,
        lesson.Type,
        lesson.IsPreview,
        lesson.IsPublished);

    private static System.Linq.Expressions.Expression<Func<Course, CourseListItemDto>> ToListItem => course =>
        new CourseListItemDto(
            course.Id,
            course.Title,
            course.Description,
            course.ThumbnailUrl,
            course.TeacherId,
            course.Teacher != null ? course.Teacher.FirstName + " " + course.Teacher.LastName : string.Empty,
            course.GradeId,
            course.SubjectId,
            course.DifficultyLevel,
            course.Price,
            course.IsFree,
            course.IsPublished,
            course.Rating,
            course.StudentCount,
            course.LessonCount,
            course.TotalDurationSeconds);
}
