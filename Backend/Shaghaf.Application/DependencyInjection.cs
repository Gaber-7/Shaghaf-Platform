using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shaghaf.Application.Features.Auth;

namespace Shaghaf.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
