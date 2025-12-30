using Microsoft.EntityFrameworkCore;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.UnitOfWork;

namespace OnTheRanj.Tests.UnitOfWorkTests;

[TestFixture]
public class UnitOfWorkTests
{
    private ApplicationDbContext _context;
    private UnitOfWork _uow;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"UnitOfWorkTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _uow = new UnitOfWork(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _uow?.Dispose();
        _context.Dispose();
    }

    [Test]
    public void Repositories_AreNotNull()
    {
        Assert.That(_uow.Users, Is.Not.Null);
        Assert.That(_uow.ProjectCodes, Is.Not.Null);
        Assert.That(_uow.ProjectAssignments, Is.Not.Null);
        Assert.That(_uow.Timesheets, Is.Not.Null);
    }

    [Test]
    public async Task CompleteAsync_SavesChanges()
    {
        var user = new OnTheRanj.Core.Entities.User { Email = "a@a.com", Role = OnTheRanj.Core.Enums.UserRole.Employee, IsActive = true };
        _uow.Users.AddAsync(user).Wait();
        var result = await _uow.CompleteAsync();
        Assert.That(result, Is.GreaterThanOrEqualTo(1));
    }


    [Test]
    public void Dispose_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _uow.Dispose());
    }
}
