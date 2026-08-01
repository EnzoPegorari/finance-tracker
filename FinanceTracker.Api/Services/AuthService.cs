using FinanceTracker.Api.Helpers;
using FinanceTracker.Api.Models.DTOs.Auth;
using FinanceTracker.Api.Models.Entities;
using FinanceTracker.Api.Repositories;

namespace FinanceTracker.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtHelper _jwtHelper;

    public AuthService(IUserRepository userRepository, IJwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new ArgumentException("Email is already registered.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken)
    {
        var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);
        if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        storedToken.IsRevoked = true;

        return await GenerateAuthResponseAsync(storedToken.User);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);
        if (storedToken is null)
            return;

        storedToken.IsRevoked = true;
        await _userRepository.SaveChangesAsync();
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(User user)
    {
        var accessToken = _jwtHelper.GenerateAccessToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();
        var refreshExpiresAt = _jwtHelper.GetRefreshTokenExpiration();
        var accessExpiresAt = _jwtHelper.GetAccessTokenExpiration();

        await _userRepository.AddRefreshTokenAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = refreshExpiresAt,
        });
        await _userRepository.SaveChangesAsync();

        return new AuthResponse(
            accessToken,
            refreshToken,
            accessExpiresAt,
            new UserDto(user.Id, user.Name, user.Email));
    }
}
