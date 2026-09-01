using Microsoft.AspNetCore.Mvc;
using Shaghaf.Application.Common.Exceptions;
using ValidationException = Shaghaf.Application.Common.Exceptions.ValidationException;

namespace Shaghaf.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await WriteProblemAsync(context, exception);
        }
    }

    private async Task WriteProblemAsync(HttpContext context, Exception exception)
    {
        var problem = exception switch
        {
            ValidationException validation => new ValidationProblemDetails(
                validation.Errors.ToDictionary(e => e.Key, e => e.Value))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = validation.Message
            },
            NotFoundException => Problem(StatusCodes.Status404NotFound, exception.Message),
            ConflictException => Problem(StatusCodes.Status409Conflict, exception.Message),
            ForbiddenException => Problem(StatusCodes.Status403Forbidden, exception.Message),
            Application.Common.Exceptions.AuthenticationException => Problem(StatusCodes.Status401Unauthorized, exception.Message),
            UnauthorizedAccessException => Problem(StatusCodes.Status401Unauthorized, "Authentication is required."),
            _ => Problem(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (problem.Status == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception while processing {Path}", context.Request.Path);
        }
        else
        {
            _logger.LogInformation("Request to {Path} failed: {Message}", context.Request.Path, exception.Message);
        }

        problem.Instance = context.Request.Path;
        context.Response.StatusCode = problem.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem, problem.GetType());
    }

    private static ProblemDetails Problem(int status, string title) => new()
    {
        Status = status,
        Title = title
    };
}
