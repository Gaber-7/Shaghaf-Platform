using Shaghaf.Domain.Entities.Users;

namespace Shaghaf.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(User user);

    /// <summary>
    /// Returns the raw refresh token handed to the client and the hash persisted on the user.
    /// </summary>
    (string Token, string Hash) CreateRefreshToken();

    string HashRefreshToken(string token);

    TimeSpan AccessTokenLifetime { get; }

    TimeSpan RefreshTokenLifetime { get; }
}
