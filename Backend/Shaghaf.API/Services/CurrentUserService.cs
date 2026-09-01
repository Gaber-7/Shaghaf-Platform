using System.Security.Claims;
using Shaghaf.Application.Common.Interfaces;
using Shaghaf.Domain.Enums;

namespace Shaghaf.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public Guid? UserId =>
        Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : null;

    public UserRole? Role =>
        Enum.TryParse<UserRole>(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role), out var role)
            ? role
            : null;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
