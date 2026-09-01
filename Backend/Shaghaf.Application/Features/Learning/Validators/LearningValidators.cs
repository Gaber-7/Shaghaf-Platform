using FluentValidation;
using Shaghaf.Application.Features.Learning.Dtos;

namespace Shaghaf.Application.Features.Learning.Validators;

public class TrackLessonProgressRequestValidator : AbstractValidator<TrackLessonProgressRequest>
{
    public TrackLessonProgressRequestValidator() =>
        RuleFor(x => x.TimeSpentSeconds).InclusiveBetween(0, 12 * 60 * 60);
}
