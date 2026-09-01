using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shaghaf.API.Authorization;
using Shaghaf.Application.Common.Models;
using Shaghaf.Application.Features.Courses;
using Shaghaf.Application.Features.Courses.Dtos;

namespace Shaghaf.API.Controllers;

[ApiController]
[Route("api/courses")]
[Produces("application/json")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService) => _courseService = courseService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<CourseListItemDto>>> Search([FromQuery] CourseQuery query, CancellationToken cancellationToken) =>
        Ok(await _courseService.SearchAsync(query, cancellationToken));

    [HttpGet("{courseId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDetailDto>> Get(Guid courseId, CancellationToken cancellationToken) =>
        Ok(await _courseService.GetAsync(courseId, cancellationToken));

    [HttpPost]
    [Authorize(Policy = Policies.TeacherOnly)]
    public async Task<ActionResult<CourseListItemDto>> Create(CreateCourseRequest request, CancellationToken cancellationToken)
    {
        var course = await _courseService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { courseId = course.Id }, course);
    }

    [HttpPut("{courseId:guid}")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<ActionResult<CourseListItemDto>> Update(Guid courseId, UpdateCourseRequest request, CancellationToken cancellationToken) =>
        Ok(await _courseService.UpdateAsync(courseId, request, cancellationToken));

    [HttpPost("{courseId:guid}/publish")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<IActionResult> Publish(Guid courseId, CancellationToken cancellationToken)
    {
        await _courseService.SetPublishedAsync(courseId, true, cancellationToken);

        return NoContent();
    }

    [HttpPost("{courseId:guid}/unpublish")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<IActionResult> Unpublish(Guid courseId, CancellationToken cancellationToken)
    {
        await _courseService.SetPublishedAsync(courseId, false, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{courseId:guid}")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<IActionResult> Delete(Guid courseId, CancellationToken cancellationToken)
    {
        await _courseService.DeleteAsync(courseId, cancellationToken);

        return NoContent();
    }

    [HttpPost("{courseId:guid}/sections")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<ActionResult<SectionDto>> AddSection(Guid courseId, CreateSectionRequest request, CancellationToken cancellationToken) =>
        Ok(await _courseService.AddSectionAsync(courseId, request, cancellationToken));

    [HttpPut("sections/{sectionId:guid}")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<ActionResult<SectionDto>> UpdateSection(Guid sectionId, UpdateSectionRequest request, CancellationToken cancellationToken) =>
        Ok(await _courseService.UpdateSectionAsync(sectionId, request, cancellationToken));

    [HttpDelete("sections/{sectionId:guid}")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<IActionResult> DeleteSection(Guid sectionId, CancellationToken cancellationToken)
    {
        await _courseService.DeleteSectionAsync(sectionId, cancellationToken);

        return NoContent();
    }

    [HttpPost("{courseId:guid}/lessons")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<ActionResult<LessonDto>> AddLesson(Guid courseId, CreateLessonRequest request, CancellationToken cancellationToken) =>
        Ok(await _courseService.AddLessonAsync(courseId, request, cancellationToken));

    [HttpPut("lessons/{lessonId:guid}")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<ActionResult<LessonDto>> UpdateLesson(Guid lessonId, UpdateLessonRequest request, CancellationToken cancellationToken) =>
        Ok(await _courseService.UpdateLessonAsync(lessonId, request, cancellationToken));

    [HttpDelete("lessons/{lessonId:guid}")]
    [Authorize(Policy = Policies.StaffOnly)]
    public async Task<IActionResult> DeleteLesson(Guid lessonId, CancellationToken cancellationToken)
    {
        await _courseService.DeleteLessonAsync(lessonId, cancellationToken);

        return NoContent();
    }
}
