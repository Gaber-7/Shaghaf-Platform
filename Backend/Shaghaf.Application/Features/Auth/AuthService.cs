using Shaghaf.Application.Common.Exceptions;
using Shaghaf.Application.Common.Interfaces;
using Shaghaf.Application.Features.Auth.Dtos;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Application.Features.Auth;

public class AuthService : IAuthService
{
    private static readonly UserRole[] SelfServiceRoles = [UserRole.Student, UserRole.Teacher, UserRole.Parent];

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (!SelfServiceRoles.Contains(request.Role))
        {
            throw new ConflictException("This role cannot be self-registered.");
        }

        var email = NormalizeEmail(request.Email);
        if (await _unitOfWork.Repository<User>().ExistsAsync(u => u.Email == email, cancellationToken))
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var user = CreateUserForRole(request, email);
        user.PasswordHash = _passwordHasher.Hash(request.Password);

        await AddUserAsync(user, cancellationToken);

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await _unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Email == email, asTracking: true, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AuthenticationException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new AuthenticationException("This account is disabled.");
        }

        user.LastLoginAt = DateTime.UtcNow;

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var hash = _tokenService.HashRefreshToken(request.RefreshToken);
        var user = await _unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.RefreshToken == hash, asTracking: true, cancellationToken);

        if (user is null || user.RefreshTokenExpiryTime is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new AuthenticationException("Invalid or expired refresh token.");
        }

        if (!user.IsActive)
        {
            throw new AuthenticationException("This account is disabled.");
        }

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetTrackedUserAsync(userId, cancellationToken);
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await GetTrackedUserAsync(userId, cancellationToken);

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new AuthenticationException("Current password is incorrect.");
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        // Changing the password invalidates existing sessions.
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), userId);

        return ToDto(user);
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken cancellationToken)
    {
        var (refreshToken, refreshTokenHash) = _tokenService.CreateRefreshToken();
        user.RefreshToken = refreshTokenHash;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.Add(_tokenService.RefreshTokenLifetime);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse(
            _tokenService.CreateAccessToken(user),
            refreshToken,
            DateTime.UtcNow.Add(_tokenService.AccessTokenLifetime),
            ToDto(user));
    }

    private async Task<User> GetTrackedUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Id == userId, asTracking: true, cancellationToken);

        return user ?? throw new NotFoundException(nameof(User), userId);
    }

    private Task AddUserAsync(User user, CancellationToken cancellationToken) => user switch
    {
        Student student => _unitOfWork.Repository<Student>().AddAsync(student, cancellationToken),
        Teacher teacher => _unitOfWork.Repository<Teacher>().AddAsync(teacher, cancellationToken),
        Parent parent => _unitOfWork.Repository<Parent>().AddAsync(parent, cancellationToken),
        _ => _unitOfWork.Repository<User>().AddAsync(user, cancellationToken)
    };

    private static User CreateUserForRole(RegisterRequest request, string email)
    {
        User user = request.Role switch
        {
            UserRole.Student => new Student { GradeId = request.GradeId, DateOfBirth = request.DateOfBirth },
            UserRole.Teacher => new Teacher(),
            UserRole.Parent => new Parent(),
            _ => throw new ConflictException("This role cannot be self-registered.")
        };

        user.Email = email;
        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.PhoneNumber = request.PhoneNumber.Trim();
        user.Role = request.Role;

        return user;
    }

    private static UserDto ToDto(User user) => new(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.PhoneNumber,
        user.Role,
        user.ProfilePictureUrl,
        user.EmailVerified);

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
