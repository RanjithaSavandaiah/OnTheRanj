using Microsoft.AspNetCore.Mvc;
using Moq;
using OnTheRanj.API.Controllers;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces.Services;
using OnTheRanj.API.DTOs;

namespace OnTheRanj.Tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock;
    private AuthController _controller;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Test]
    public async Task Login_ReturnsOk_OnSuccess()
    {
        _authServiceMock.Setup(s => s.LoginAsync("a@a.com", "pass")).ReturnsAsync("token");
        var req = new LoginRequest { Email = "a@a.com", Password = "pass" };
        var result = await _controller.Login(req) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((LoginResponse)result!.Value!).Token, Is.EqualTo("token"));
    }

    [Test]
    public async Task Login_ReturnsUnauthorized_OnFailure()
    {
        _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new UnauthorizedAccessException("fail"));
        var req = new LoginRequest { Email = "a@a.com", Password = "bad" };
        var result = await _controller.Login(req) as UnauthorizedObjectResult;
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Register_ReturnsCreated_OnSuccess()
    {
        var user = new User { Id = 1, FullName = "Test", Email = "a@a.com", Role = "Employee" };
        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
        var req = new RegisterRequest { FullName = "Test", Email = "a@a.com", Password = "pw", Role = "Employee" };
        var result = await _controller.Register(req) as CreatedAtActionResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(((RegisterResponse)result!.Value!).UserId, Is.EqualTo(1));
    }

    [Test]
    public async Task Register_ReturnsBadRequest_OnFailure()
    {
        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new InvalidOperationException("fail"));
        var req = new RegisterRequest { FullName = "Test", Email = "a@a.com", Password = "pw", Role = "Employee" };
        var result = await _controller.Register(req) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
    }
}
