using Microsoft.Extensions.Configuration;
using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Services;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IUserRepository> _userRepoMock;
    private Mock<IConfiguration> _configMock;
    private AuthService _service;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepoMock = new Mock<IUserRepository>();
        _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
        _configMock = new Mock<IConfiguration>();
        _configMock.Setup(c => c["Jwt:SecretKey"]).Returns("TestSecretKeyTestSecretKeyTestSecretKeyTestSecretKey");
        _configMock.Setup(c => c["Jwt:ExpiryHours"]).Returns("1");
        _service = new AuthService(_unitOfWorkMock.Object, _configMock.Object);
    }

    [Test]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsValid()
    {
        var user = new User { Id = 1, Email = "test@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass"), IsActive = true, Role = "Employee", FullName = "Test User" };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
        var token = await _service.LoginAsync("test@example.com", "pass");
        Assert.That(token, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void LoginAsync_Throws_WhenUserNotFound()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null!);
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.LoginAsync("nouser@example.com", "pass"));
    }

    [Test]
    public void LoginAsync_Throws_WhenUserInactive()
    {
        var user = new User { Id = 1, Email = "test@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass"), IsActive = false, Role = "Employee", FullName = "Test User" };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.LoginAsync("test@example.com", "pass"));
    }

    [Test]
    public void LoginAsync_Throws_WhenPasswordWrong()
    {
        var user = new User { Id = 1, Email = "test@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass"), IsActive = true, Role = "Employee", FullName = "Test User" };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.LoginAsync("test@example.com", "wrong"));
    }

    [Test]
    public async Task RegisterAsync_CreatesUser_WhenEmailNotExists()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null!);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
        var user = await _service.RegisterAsync("New User", "new@example.com", "pass", "Employee");
        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Email, Is.EqualTo("new@example.com"));
        Assert.That(user.PasswordHash, Is.Not.EqualTo("pass"));
    }

    [Test]
    public void RegisterAsync_Throws_WhenEmailExists()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("exists@example.com")).ReturnsAsync(new User { Email = "exists@example.com" });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.RegisterAsync("User", "exists@example.com", "pass", "Employee"));
    }

    [Test]
    public void ValidateToken_ReturnsTrue_ForValidToken()
    {
        var user = new User { Id = 1, Email = "test@example.com", FullName = "Test User", Role = "Employee" };
        var token = typeof(AuthService).GetMethod("GenerateJwtToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(_service, new object[] { user }) as string;
        var valid = _service.ValidateToken(token!);
        Assert.That(valid, Is.True);
    }

    [Test]
    public void ValidateToken_ReturnsFalse_ForInvalidToken()
    {
        var valid = _service.ValidateToken("invalid.token.value");
        Assert.That(valid, Is.False);
    }

    [Test]
    public void GetUserIdFromToken_ReturnsId()
    {
        var password = "pass42";
        var user = new User { Id = 42, Email = "test42@example.com", FullName = "Test User", Role = "Employee", PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), IsActive = true };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test42@example.com")).ReturnsAsync(user);
        var token = _service.LoginAsync("test42@example.com", password).GetAwaiter().GetResult();
        Assert.That(_service.ValidateToken(token), Is.True, "Token should be valid");
        var id = _service.GetUserIdFromToken(token);
        Assert.That(id, Is.EqualTo(42));
    }

    [Test]
    public void GetUserIdFromToken_Throws_ForInvalidToken()
    {
        Assert.Throws<InvalidOperationException>(() => _service.GetUserIdFromToken("invalid.token.value"));
    }

    [Test]
    public void GetUserIdFromToken_Throws_WhenClaimMissing()
    {
        // Create a token with no sub/userId claim
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = handler.WriteToken(new System.IdentityModel.Tokens.Jwt.JwtSecurityToken());
        Assert.Throws<InvalidOperationException>(() => _service.GetUserIdFromToken(token));
    }

    [Test]
    public void AuthService_UsesDefaultSecret_WhenConfigMissingSecret()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["Jwt:SecretKey"]).Returns((string?)null);
        config.Setup(c => c["Jwt:ExpiryHours"]).Returns("1");
        var service = new AuthService(_unitOfWorkMock.Object, config.Object);
        var secretField = typeof(AuthService).GetField("_jwtSecret", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var secret = (string)secretField!.GetValue(service)!;
        Assert.That(secret, Is.EqualTo("YourSuperSecretKeyForJWTTokenGenerationWithMinimum256Bits"));
    }

    [Test]
    public void AuthService_UsesDefaultExpiry_WhenConfigMissingExpiry()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["Jwt:SecretKey"]).Returns("TestSecretKeyTestSecretKeyTestSecretKeyTestSecretKey");
        config.Setup(c => c["Jwt:ExpiryHours"]).Returns((string?)null);
        var service = new AuthService(_unitOfWorkMock.Object, config.Object);
        var expiryField = typeof(AuthService).GetField("_jwtExpiryHours", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var expiry = (int)expiryField!.GetValue(service)!;
        Assert.That(expiry, Is.EqualTo(24));
    }
}
