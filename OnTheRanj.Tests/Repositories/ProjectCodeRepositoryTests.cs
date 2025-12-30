using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Repositories;

namespace OnTheRanj.Tests.Repositories;

[TestFixture]
public class ProjectCodeRepositoryTests
{
    private ApplicationDbContext _context;
    private ProjectCodeRepository _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"ProjectCodeRepoTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _repo = new ProjectCodeRepository(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    [Test]
    public async Task GetActiveProjectCodesAsync_ReturnsOnlyActive()
    {
        _context.ProjectCodes.Add(new ProjectCode { Code = "A", Status = ProjectStatus.Active });
        _context.ProjectCodes.Add(new ProjectCode { Code = "B", Status = ProjectStatus.Inactive });
        await _context.SaveChangesAsync();
        var active = await _repo.GetActiveProjectCodesAsync();
        Assert.That(active.All(pc => pc.Status == ProjectStatus.Active), Is.True);
    }

    [Test]
    public async Task GetByCodeAsync_ReturnsProjectCode()
    {
        _context.ProjectCodes.Add(new ProjectCode { Code = "X", Status = ProjectStatus.Active });
        await _context.SaveChangesAsync();
        var found = await _repo.GetByCodeAsync("X");
        Assert.That(found, Is.Not.Null);
    }

    [Test]
    public async Task GetByManagerIdAsync_ReturnsOnlyByManager()
    {
        _context.ProjectCodes.Add(new ProjectCode { Code = "A", Status = ProjectStatus.Active, CreatedBy = 1 });
        _context.ProjectCodes.Add(new ProjectCode { Code = "B", Status = ProjectStatus.Active, CreatedBy = 2 });
        await _context.SaveChangesAsync();
        var codes = await _repo.GetByManagerIdAsync(1);
        Assert.That(codes.All(pc => pc.CreatedBy == 1), Is.True);
    }
}
