using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Repositories;

namespace OnTheRanj.Tests.Repositories;

[TestFixture]
public class TimesheetRepositoryTests
{
    private ApplicationDbContext _context;
    private TimesheetRepository _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"TimesheetRepoTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _repo = new TimesheetRepository(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    [Test]
    public async Task GetByEmployeeIdAsync_ReturnsTimesheets()
    {
        _context.Timesheets.Add(new Timesheet { Id = 1, EmployeeId = 1, Date = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var result = await _repo.GetByEmployeeIdAsync(1);
        Assert.That(result.All(t => t.EmployeeId == 1), Is.True);
    }

    [Test]
    public async Task GetByProjectCodeIdAsync_ReturnsTimesheets()
    {
        _context.Timesheets.Add(new Timesheet { Id = 2, ProjectCodeId = 5, Date = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var result = await _repo.GetByProjectCodeIdAsync(5);
        Assert.That(result.All(t => t.ProjectCodeId == 5), Is.True);
    }

    [Test]
    public async Task GetByStatusAsync_ReturnsTimesheets()
    {
        _context.Timesheets.Add(new Timesheet { Id = 3, Status = TimesheetStatus.Submitted, Date = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var result = await _repo.GetByStatusAsync(TimesheetStatus.Submitted);
        Assert.That(result.All(t => t.Status == TimesheetStatus.Submitted), Is.True);
    }

    [Test]
    public async Task GetByDateRangeAsync_ReturnsTimesheets()
    {
        var today = DateTime.UtcNow.Date;
        // Add required related entities
        var user = new User { Id = 1, Email = "a@a.com", Role = UserRole.Employee, IsActive = true };
        var projectCode = new ProjectCode { Id = 1, Code = "P1", Status = ProjectStatus.Active };
        _context.Users.Add(user);
        _context.ProjectCodes.Add(projectCode);
        _context.Timesheets.Add(new Timesheet { Id = 4, Date = today, EmployeeId = 1, ProjectCodeId = 1 });
        _context.Timesheets.Add(new Timesheet { Id = 5, Date = today.AddDays(-10), EmployeeId = 1, ProjectCodeId = 1 });
        await _context.SaveChangesAsync();
        var result = await _repo.GetByDateRangeAsync(today.AddDays(-15), today.AddDays(1));
        Assert.That(result.Any(), Is.True);
    }

    [Test]
    public async Task GetByEmployeeAndDateRangeAsync_ReturnsTimesheets()
    {
        var today = DateTime.UtcNow.Date;
        _context.Timesheets.Add(new Timesheet { Id = 6, EmployeeId = 7, Date = today });
        await _context.SaveChangesAsync();
        var result = await _repo.GetByEmployeeAndDateRangeAsync(7, today.AddDays(-1), today.AddDays(1));
        Assert.That(result.All(t => t.EmployeeId == 7), Is.True);
    }

    [Test]
    public async Task GetTotalHoursForDateAsync_ReturnsSum()
    {
        var today = DateTime.UtcNow.Date;
        _context.Timesheets.Add(new Timesheet { Id = 8, EmployeeId = 9, Date = today, HoursWorked = 5 });
        _context.Timesheets.Add(new Timesheet { Id = 9, EmployeeId = 9, Date = today, HoursWorked = 3 });
        await _context.SaveChangesAsync();
        var sum = await _repo.GetTotalHoursForDateAsync(9, today);
        Assert.That(sum, Is.EqualTo(8));
    }

    [Test]
    public async Task HasDuplicateEntryAsync_ReturnsTrueIfDuplicate()
    {
        var today = DateTime.UtcNow.Date;
        _context.Timesheets.Add(new Timesheet { Id = 10, EmployeeId = 11, ProjectCodeId = 12, Date = today });
        await _context.SaveChangesAsync();
        var result = await _repo.HasDuplicateEntryAsync(11, 12, today);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task GetPendingTimesheetsAsync_ReturnsSubmitted()
    {
        _context.Timesheets.Add(new Timesheet { Id = 13, Status = TimesheetStatus.Submitted, Date = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        var result = await _repo.GetPendingTimesheetsAsync();
        Assert.That(result.All(t => t.Status == TimesheetStatus.Submitted), Is.True);
    }
}
