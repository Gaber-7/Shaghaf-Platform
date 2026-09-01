using Shaghaf.Application.Common.Models;
using Shaghaf.Application.Features.Learning.Dtos;

namespace Shaghaf.Application.Features.Learning;

public interface IEnrollmentService
{
    Task<EnrollmentDto> EnrollAsync(Guid courseId, CancellationToken cancellationToken = default);

    Task<PagedResult<EnrollmentDto>> GetMyEnrollmentsAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<CourseProgressDto> GetCourseProgressAsync(Guid courseId, CancellationToken cancellationToken = default);

    Task<LessonProgressDto> TrackLessonAsync(Guid lessonId, TrackLessonProgressRequest request, CancellationToken cancellationToken = default);
}
