using Microsoft.AspNetCore.Authorization;
using Shaghaf.Domain.Enums;

namespace Shaghaf.API.Authorization;

public static class Policies
{
    public const string AdminOnly = nameof(AdminOnly);
    public const string TeacherOnly = nameof(TeacherOnly);
    public const string StudentOnly = nameof(StudentOnly);
    public const string ParentOnly = nameof(ParentOnly);
    public const string StaffOnly = nameof(StaffOnly);

    public static AuthorizationOptions AddShaghafPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(AdminOnly, p => p.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin)));
        options.AddPolicy(TeacherOnly, p => p.RequireRole(nameof(UserRole.Teacher)));
        options.AddPolicy(StudentOnly, p => p.RequireRole(nameof(UserRole.Student)));
        options.AddPolicy(ParentOnly, p => p.RequireRole(nameof(UserRole.Parent)));
        options.AddPolicy(StaffOnly, p => p.RequireRole(
            nameof(UserRole.SuperAdmin), nameof(UserRole.Admin), nameof(UserRole.Teacher)));

        options.FallbackPolicy = null;
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        return options;
    }
}
