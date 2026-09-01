using Shaghaf.Application.Common.Models;
using Shaghaf.Application.Features.Courses.Dtos;

namespace Shaghaf.Application.Features.Courses;

public interface ICourseService
{
    Task<PagedResult<CourseListItemDto>> SearchAsync(CourseQuery query, CancellationToken cancellationToken = default);

    Task<CourseDetailDto> GetAsync(Guid courseId, CancellationToken cancellationToken = default);

    Task<CourseListItemDto> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default);

    Task<CourseListItemDto> UpdateAsync(Guid courseId, UpdateCourseRequest request, CancellationToken cancellationToken = default);

    Task SetPublishedAsync(Guid courseId, bool isPublished, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid courseId, CancellationToken cancellationToken = default);

    Task<SectionDto> AddSectionAsync(Guid courseId, CreateSectionRequest request, CancellationToken cancellationToken = default);

    Task<SectionDto> UpdateSectionAsync(Guid sectionId, UpdateSectionRequest request, CancellationToken cancellationToken = default);

    Task DeleteSectionAsync(Guid sectionId, CancellationToken cancellationToken = default);

    Task<LessonDto> AddLessonAsync(Guid courseId, CreateLessonRequest request, CancellationToken cancellationToken = default);

    Task<LessonDto> UpdateLessonAsync(Guid lessonId, UpdateLessonRequest request, CancellationToken cancellationToken = default);

    Task DeleteLessonAsync(Guid lessonId, CancellationToken cancellationToken = default);
}
