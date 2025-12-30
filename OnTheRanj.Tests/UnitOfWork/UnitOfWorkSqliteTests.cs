using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.UnitOfWork;

namespace OnTheRanj.Tests.UnitOfWorkTests;

[TestFixture]
public class UnitOfWorkSqliteTests
{
    private ApplicationDbContext _context;
    private UnitOfWork _uow;
    private SqliteConnection _connection;

    [SetUp]
    public void Setup()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        _uow = new UnitOfWork(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _uow?.Dispose();
        _context?.Dispose();
        _connection?.Dispose();
    }

    [Test]
    public async Task Transaction_CommitsAndRollsBack()
    {
        await _uow.BeginTransactionAsync();
        var user = new Core.Entities.User { Email = "b@b.com", Role = Core.Enums.UserRole.Employee, IsActive = true };
        await _uow.Users.AddAsync(user);
        await _uow.CompleteAsync();
        await _uow.CommitTransactionAsync();
        var found = await _uow.Users.GetByEmailAsync("b@b.com");
        Assert.That(found, Is.Not.Null);

        await _uow.BeginTransactionAsync();
        var user2 = new Core.Entities.User { Email = "c@c.com", Role = Core.Enums.UserRole.Employee, IsActive = true };
        await _uow.Users.AddAsync(user2);
        await _uow.CompleteAsync();
        await _uow.RollbackTransactionAsync();
        var found2 = await _uow.Users.GetByEmailAsync("c@c.com");
        Assert.That(found2, Is.Null);
    }
}
