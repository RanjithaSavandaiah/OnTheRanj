using Microsoft.EntityFrameworkCore;
using OnTheRanj.API.Data;
using OnTheRanj.Core.Entities;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.Tests.Data;

public class DbSeederTests
{
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void SeedUsers_Adds_Initial_Users_If_Empty()
    {
        // Act
        DbSeeder.SeedUsers(_context);

        // Assert
        var users = _context.Users.ToList();
        Assert.That(users.Count, Is.EqualTo(3));
        Assert.That(users.Any(u => u.Role == "Manager"));
        Assert.That(users.Any(u => u.Role == "Employee"));
    }

    [Test]
    public void SeedUsers_Does_Not_Duplicate_If_Already_Seeded()
    {
        DbSeeder.SeedUsers(_context);
        DbSeeder.SeedUsers(_context);
        var users = _context.Users.ToList();
        Assert.That(users.Count, Is.EqualTo(3));
    }

    [Test]
    public void SeedTimesheets_Adds_Initial_Timesheets_If_Empty()
    {
        // Arrange
        _context.Users.Add(new User { Id = 2, FullName = "Alice Employee", Email = "alice@ontheranj.com", PasswordHash = "x", Role = "Employee", IsActive = true, CreatedAt = DateTime.UtcNow });
        _context.Users.Add(new User { Id = 3, FullName = "Bob Employee", Email = "bob@ontheranj.com", PasswordHash = "x", Role = "Employee", IsActive = true, CreatedAt = DateTime.UtcNow });
        _context.ProjectCodes.Add(new ProjectCode { Id = 1, ProjectName = "Frontend" });
        _context.ProjectCodes.Add(new ProjectCode { Id = 2, ProjectName = "API" });
        _context.ProjectCodes.Add(new ProjectCode { Id = 4, ProjectName = "Internal" });
        _context.SaveChanges();

        // Act
        DbSeeder.SeedTimesheets(_context);

        // Assert
        var timesheets = _context.Timesheets.ToList();
        Assert.That(timesheets.Count, Is.EqualTo(5));
        Assert.That(timesheets.Any(t => t.EmployeeId == 2));
        Assert.That(timesheets.Any(t => t.EmployeeId == 3));
    }

    [Test]
    public void SeedTimesheets_Does_Not_Duplicate_If_Already_Seeded()
    {
        _context.Users.Add(new User { Id = 2, FullName = "Alice Employee", Email = "alice@ontheranj.com", PasswordHash = "x", Role = "Employee", IsActive = true, CreatedAt = DateTime.UtcNow });
        _context.Users.Add(new User { Id = 3, FullName = "Bob Employee", Email = "bob@ontheranj.com", PasswordHash = "x", Role = "Employee", IsActive = true, CreatedAt = DateTime.UtcNow });
        _context.ProjectCodes.Add(new ProjectCode { Id = 1, ProjectName = "Frontend" });
        _context.ProjectCodes.Add(new ProjectCode { Id = 2, ProjectName = "API" });
        _context.ProjectCodes.Add(new ProjectCode { Id = 4, ProjectName = "Internal" });
        _context.SaveChanges();

        DbSeeder.SeedTimesheets(_context);
        DbSeeder.SeedTimesheets(_context);
        var timesheets = _context.Timesheets.ToList();
        Assert.That(timesheets.Count, Is.EqualTo(5));
    }
}
