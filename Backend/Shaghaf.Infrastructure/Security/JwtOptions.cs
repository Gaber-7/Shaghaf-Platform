using System.ComponentModel.DataAnnotations;

namespace Shaghaf.Infrastructure.Security;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    [MinLength(32)]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, 1440)]
    public int ExpirationMinutes { get; set; } = 15;

    [Range(1, 365)]
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
