using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Repositories;

namespace OnTheRanj.Tests.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    private ApplicationDbContext _context;
    private UserRepository _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"UserRepoTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _repo = new UserRepository(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    [Test]
    public async Task GetByEmailAsync_ReturnsUser()
    {
        var user = new User { Email = "a@a.com", Role = UserRole.Employee, IsActive = true };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var found = await _repo.GetByEmailAsync("a@a.com");
        Assert.That(found, Is.Not.Null);
    }

    [Test]
    public async Task GetByEmailAsync_ReturnsNull_WhenNotFound()
    {
        var found = await _repo.GetByEmailAsync("notfound@x.com");
        Assert.That(found, Is.Null);
    }

    [Test]
    public async Task GetEmployeesAsync_ReturnsOnlyEmployees()
    {
        _context.Users.Add(new User { Email = "e@e.com", Role = UserRole.Employee, IsActive = true });
        _context.Users.Add(new User { Email = "m@m.com", Role = UserRole.Manager, IsActive = true });
        await _context.SaveChangesAsync();
        var employees = await _repo.GetEmployeesAsync();
        Assert.That(employees.All(u => u.Role == UserRole.Employee), Is.True);
    }

    [Test]
    public async Task GetManagersAsync_ReturnsOnlyManagers()
    {
        _context.Users.Add(new User { Email = "e@e.com", Role = UserRole.Employee, IsActive = true });
        _context.Users.Add(new User { Email = "m@m.com", Role = UserRole.Manager, IsActive = true });
        await _context.SaveChangesAsync();
        var managers = await _repo.GetManagersAsync();
        Assert.That(managers.All(u => u.Role == UserRole.Manager), Is.True);
    }
}
