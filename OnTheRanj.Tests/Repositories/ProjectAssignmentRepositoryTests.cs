using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Repositories;

namespace OnTheRanj.Tests.Repositories;

[TestFixture]
public class ProjectAssignmentRepositoryTests
{
    private ApplicationDbContext _context;
    private ProjectAssignmentRepository _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"ProjectAssignmentRepoTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _repo = new ProjectAssignmentRepository(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    [Test]
    public async Task GetAllProjectAssignmentsAsync_ReturnsAll()
    {
        // Add required related entities
        var user = new User { Id = 1, Email = "a@a.com", Role = UserRole.Employee, IsActive = true };
        var projectCode = new ProjectCode { Id = 1, Code = "P1", Status = ProjectStatus.Active };
        _context.Users.Add(user);
        _context.ProjectCodes.Add(projectCode);
        _context.ProjectAssignments.Add(new ProjectAssignment { Id = 1, ProjectCodeId = 1, EmployeeId = 1, CreatedAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var all = await _repo.GetAllProjectAssignmentsAsync();
        Assert.That(all.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetActiveAssignmentsByEmployeeIdAsync_ReturnsActive()
    {
        var today = DateTime.UtcNow.Date;
        _context.ProjectCodes.Add(new ProjectCode { Id = 1, Status = ProjectStatus.Active });
        _context.Users.Add(new User { Id = 1, Role = UserRole.Employee, IsActive = true });
        _context.ProjectAssignments.Add(new ProjectAssignment { Id = 2, EmployeeId = 1, ProjectCodeId = 1, StartDate = today.AddDays(-1), EndDate = today.AddDays(1), CreatedAt = today });
        await _context.SaveChangesAsync();
        var active = await _repo.GetActiveAssignmentsByEmployeeIdAsync(1);
        Assert.That(active.Any(), Is.True);
    }

    [Test]
    public async Task GetAssignmentsByEmployeeIdAsync_ReturnsAssignments()
    {
        _context.ProjectAssignments.Add(new ProjectAssignment { Id = 3, EmployeeId = 2, ProjectCodeId = 2, CreatedAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var result = await _repo.GetAssignmentsByEmployeeIdAsync(2);
        Assert.That(result.All(a => a.EmployeeId == 2), Is.True);
    }

    [Test]
    public async Task GetAssignmentsByProjectCodeIdAsync_ReturnsAssignments()
    {
        _context.ProjectAssignments.Add(new ProjectAssignment { Id = 4, EmployeeId = 3, ProjectCodeId = 5, CreatedAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var result = await _repo.GetAssignmentsByProjectCodeIdAsync(5);
        Assert.That(result.All(a => a.ProjectCodeId == 5), Is.True);
    }

    [Test]
    public async Task IsEmployeeAssignedToProjectAsync_ReturnsTrueIfAssigned()
    {
        var today = DateTime.UtcNow.Date;
        _context.ProjectAssignments.Add(new ProjectAssignment { Id = 5, EmployeeId = 10, ProjectCodeId = 20, StartDate = today.AddDays(-1), EndDate = today.AddDays(1), CreatedAt = today });
        await _context.SaveChangesAsync();
        var result = await _repo.IsEmployeeAssignedToProjectAsync(10, 20, today);
        Assert.That(result, Is.True);
    }
}
