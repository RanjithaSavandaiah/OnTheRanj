using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Services;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private ApplicationDbContext _context;
    private UserService _service;

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"UserServiceTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _service = new UserService(_context);
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        _context.Users.Add(new User { Id = 1, Email = "a@a.com" });
        _context.Users.Add(new User { Id = 2, Email = "b@b.com" });
        await _context.SaveChangesAsync();
        var users = await _service.GetAllUsersAsync();
        Assert.That(users.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetUserByIdAsync_ReturnsUser()
    {
        var user = new User { Id = 10, Email = "x@x.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var found = await _service.GetUserByIdAsync(10);
        Assert.That(found, Is.Not.Null);
        Assert.That(found.Email, Is.EqualTo("x@x.com"));
    }

    [Test]
    public async Task GetUserByIdAsync_ReturnsNull_WhenNotFound()
    {
        var found = await _service.GetUserByIdAsync(999);
        Assert.That(found, Is.Null);
    }

    [Test]
    public async Task CreateUserAsync_AddsUser()
    {
        var user = new User { Id = 100, Email = "new@user.com" };
        var created = await _service.CreateUserAsync(user);
        Assert.That(created, Is.Not.Null);
        Assert.That(_context.Users.Any(u => u.Email == "new@user.com"), Is.True);
    }

    [Test]
    public async Task UpdateUserAsync_UpdatesUser()
    {
        var user = new User { Id = 200, Email = "old@user.com", FullName = "Old" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        user.FullName = "New";
        var updated = await _service.UpdateUserAsync(user);
        Assert.That(updated.FullName, Is.EqualTo("New"));
    }

    [Test]
    public async Task DeleteUserAsync_RemovesUser()
    {
        var user = new User { Id = 300, Email = "del@user.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var result = await _service.DeleteUserAsync(300);
        Assert.That(result, Is.True);
        Assert.That(_context.Users.Any(u => u.Id == 300), Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_ReturnsFalse_WhenNotFound()
    {
        var result = await _service.DeleteUserAsync(9999);
        Assert.That(result, Is.False);
    }
}
