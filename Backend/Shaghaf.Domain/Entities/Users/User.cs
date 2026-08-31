using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Base User entity - inherited by Student, Parent, Teacher, Admin
/// </summary>
public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string GetFullName() => $"{FirstName} {LastName}";
}
