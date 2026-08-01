using FinanceTracker.Api.Helpers;
using FinanceTracker.Api.Models.DTOs.Auth;
using FinanceTracker.Api.Models.Entities;
using FinanceTracker.Api.Repositories;
using FinanceTracker.Api.Services;
using Moq;
using Xunit;

namespace FinanceTracker.Api.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IJwtHelper> _jwtHelper = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _jwtHelper.Setup(j => j.GenerateAccessToken(It.IsAny<User>())).Returns("access-token");
        _jwtHelper.Setup(j => j.GenerateRefreshToken()).Returns("refresh-token");
        _jwtHelper.Setup(j => j.GetRefreshTokenExpiration()).Returns(DateTime.UtcNow.AddDays(7));
        _jwtHelper.Setup(j => j.GetAccessTokenExpiration()).Returns(DateTime.UtcNow.AddMinutes(15));

        _sut = new AuthService(_userRepository.Object, _jwtHelper.Object);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsArgumentException()
    {
        _userRepository.Setup(r => r.GetByEmailAsync("taken@example.com"))
            .ReturnsAsync(new User { Email = "taken@example.com" });

        var request = new RegisterRequest("Enzo", "taken@example.com", "password123");

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.RegisterAsync(request));
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailIsNew_CreatesUserAndReturnsTokens()
    {
        _userRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var request = new RegisterRequest("Enzo", "new@example.com", "password123");

        var result = await _sut.RegisterAsync(request);

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.Equal("new@example.com", result.User.Email);
        _userRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ThrowsUnauthorizedAccessException()
    {
        var user = new User
        {
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct-password"),
        };
        _userRepository.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

        var request = new LoginRequest(user.Email, "wrong-password");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ThrowsUnauthorizedAccessException()
    {
        _userRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var request = new LoginRequest("ghost@example.com", "password123");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.LoginAsync(request));
    }

    [Fact]
    public async Task RefreshAsync_WhenTokenIsExpired_ThrowsUnauthorizedAccessException()
    {
        var expiredToken = new RefreshToken
        {
            Token = "expired-token",
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            User = new User { Email = "user@example.com" },
        };
        _userRepository.Setup(r => r.GetRefreshTokenAsync("expired-token")).ReturnsAsync(expiredToken);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.RefreshAsync("expired-token"));
    }

    [Fact]
    public async Task RefreshAsync_WhenTokenIsRevoked_ThrowsUnauthorizedAccessException()
    {
        var revokedToken = new RefreshToken
        {
            Token = "revoked-token",
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            IsRevoked = true,
            User = new User { Email = "user@example.com" },
        };
        _userRepository.Setup(r => r.GetRefreshTokenAsync("revoked-token")).ReturnsAsync(revokedToken);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.RefreshAsync("revoked-token"));
    }
}
