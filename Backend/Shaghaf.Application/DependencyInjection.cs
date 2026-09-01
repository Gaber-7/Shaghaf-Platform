using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shaghaf.Application.Features.Auth;
using Shaghaf.Application.Features.Courses;
using Shaghaf.Application.Features.Learning;

namespace Shaghaf.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();

        return services;
    }
}
