using Microsoft.EntityFrameworkCore;
using OnTheRanj.Infrastructure.Repositories;

namespace OnTheRanj.Tests.Repositories;


public class DummyEntity { public int Id { get; set; } public string Name { get; set; } = string.Empty; }

public class TestDbContext : OnTheRanj.Infrastructure.Data.ApplicationDbContext
{
    public TestDbContext(DbContextOptions<OnTheRanj.Infrastructure.Data.ApplicationDbContext> options) : base(options) { }
    public DbSet<DummyEntity> DummyEntities { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DummyEntity>().HasKey(e => e.Id);
    }
}

[TestFixture]
public class RepositoryTests
{

    private TestDbContext _context;
    private Repository<DummyEntity> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<OnTheRanj.Infrastructure.Data.ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
            .Options;
        _context = new TestDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new Repository<DummyEntity>(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void Query_ReturnsQueryable()
    {
        var query = _repository.Query();
        Assert.That(query, Is.Not.Null);
    }

    [Test]
    public async Task AddAsync_AddsEntity()
    {
        var entity = new DummyEntity { Id = 1, Name = "Test" };
        var result = await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        Assert.That(result, Is.EqualTo(entity));
        Assert.That(_context.Set<DummyEntity>().Find(1), Is.Not.Null);
    }

    [Test]
    public async Task UpdateAsync_UpdatesEntity()
    {
        var entity = new DummyEntity { Id = 2, Name = "Update" };
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        entity.Name = "Updated";
        await _repository.UpdateAsync(entity);
        await _context.SaveChangesAsync();
        var updated = await _context.Set<DummyEntity>().FindAsync(2);
        Assert.That(updated!.Name, Is.EqualTo("Updated"));
    }

    [Test]
    public async Task DeleteAsync_DeletesEntity()
    {
        var entity = new DummyEntity { Id = 3, Name = "Delete" };
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _repository.DeleteAsync(entity);
        await _context.SaveChangesAsync();
        var deleted = await _context.Set<DummyEntity>().FindAsync(3);
        Assert.That(deleted, Is.Null);
    }
}
