using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shaghaf.API.Authorization;
using Shaghaf.Application.Common.Models;
using Shaghaf.Application.Features.Learning;
using Shaghaf.Application.Features.Learning.Dtos;

namespace Shaghaf.API.Controllers;

[ApiController]
[Route("api/enrollments")]
[Produces("application/json")]
[Authorize(Policy = Policies.StudentOnly)]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService) => _enrollmentService = enrollmentService;

    [HttpPost("courses/{courseId:guid}")]
    public async Task<ActionResult<EnrollmentDto>> Enroll(Guid courseId, CancellationToken cancellationToken) =>
        Ok(await _enrollmentService.EnrollAsync(courseId, cancellationToken));

    [HttpGet("me")]
    public async Task<ActionResult<PagedResult<EnrollmentDto>>> MyEnrollments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default) =>
        Ok(await _enrollmentService.GetMyEnrollmentsAsync(page, pageSize, cancellationToken));

    [HttpGet("courses/{courseId:guid}/progress")]
    public async Task<ActionResult<CourseProgressDto>> Progress(Guid courseId, CancellationToken cancellationToken) =>
        Ok(await _enrollmentService.GetCourseProgressAsync(courseId, cancellationToken));

    [HttpPost("lessons/{lessonId:guid}/progress")]
    public async Task<ActionResult<LessonProgressDto>> TrackLesson(
        Guid lessonId,
        TrackLessonProgressRequest request,
        CancellationToken cancellationToken) =>
        Ok(await _enrollmentService.TrackLessonAsync(lessonId, request, cancellationToken));
}
