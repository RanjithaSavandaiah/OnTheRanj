using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Services.Implementations;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class TimesheetServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private TimesheetService _service;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new TimesheetService(_unitOfWorkMock.Object);
        // Only set up methods here that are used in every test and return Task (not Task<T>)
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
    }
    [Test]
    public void UpdateTimesheetAsync_Throws_WhenRepositoryThrows()
    {
        var timesheet = new Timesheet { Id = 1 };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ThrowsAsync(new Exception("DB error"));
        Assert.ThrowsAsync<Exception>(async () => await _service.UpdateTimesheetAsync(timesheet));
    }

    [Test]
    public void SubmitTimesheetAsync_Throws_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ThrowsAsync(new Exception("DB error"));
        Assert.ThrowsAsync<Exception>(async () => await _service.SubmitTimesheetAsync(1));
    }

    [Test]
    public void DeleteTimesheetAsync_Throws_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ThrowsAsync(new Exception("DB error"));
        Assert.ThrowsAsync<Exception>(async () => await _service.DeleteTimesheetAsync(1));
    }

    [Test]
    public void GetEmployeeTimesheetsAsync_Throws_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByEmployeeIdAsync(1)).ThrowsAsync(new Exception("DB error"));
        Assert.ThrowsAsync<Exception>(async () => await _service.GetEmployeeTimesheetsAsync(1));
    }

    [Test]
    public void GetTimesheetByIdAsync_Throws_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ThrowsAsync(new Exception("DB error"));
        Assert.ThrowsAsync<Exception>(async () => await _service.GetTimesheetByIdAsync(1));
    }


    [Test]
    public async Task GetAllTimesheetsAsync_ReturnsTimesheets()
    {
        var timesheets = new List<Timesheet> { new Timesheet { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetAllAsync()).ReturnsAsync(timesheets);
        var result = await _service.GetAllTimesheetsAsync();
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetAllTimesheetsAsync_ReturnsEmptyList_WhenNoTimesheets()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetAllAsync()).ReturnsAsync(new List<Timesheet>());
        var result = await _service.GetAllTimesheetsAsync();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllTimesheetsAsync_ThrowsException_WhenRepositoryThrows()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetAllAsync()).ThrowsAsync(new Exception("DB error"));
        Assert.ThrowsAsync<Exception>(async () => await _service.GetAllTimesheetsAsync());
    }

    [Test]
    public async Task CreateTimesheetAsync_SetsDraftAndCallsAdd()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 8, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetTotalHoursForDateAsync(1, timesheet.Date)).ReturnsAsync(0);
        _unitOfWorkMock.Setup(u => u.Timesheets.HasDuplicateEntryAsync(1, 2, timesheet.Date, null)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(1, 2, timesheet.Date)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Active });
        var result = await _service.CreateTimesheetAsync(timesheet);
        Assert.That(result.Status, Is.EqualTo(TimesheetStatus.Draft));
        _unitOfWorkMock.Verify(u => u.Timesheets.AddAsync(timesheet), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public void CreateTimesheetAsync_Throws_WhenHoursInvalid()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 25, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today };
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateTimesheetAsync(timesheet));
    }

    [Test]
    public void CreateTimesheetAsync_Throws_WhenTotalHoursExceed()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 10, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetTotalHoursForDateAsync(1, timesheet.Date)).ReturnsAsync(20);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateTimesheetAsync(timesheet));
    }

    [Test]
    public void CreateTimesheetAsync_Throws_WhenDuplicateEntry()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 8, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetTotalHoursForDateAsync(1, timesheet.Date)).ReturnsAsync(0);
        _unitOfWorkMock.Setup(u => u.Timesheets.HasDuplicateEntryAsync(1, 2, timesheet.Date, null)).ReturnsAsync(true);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateTimesheetAsync(timesheet));
    }

    [Test]
    public void CreateTimesheetAsync_Throws_WhenNotAssigned()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 8, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetTotalHoursForDateAsync(1, timesheet.Date)).ReturnsAsync(0);
        _unitOfWorkMock.Setup(u => u.Timesheets.HasDuplicateEntryAsync(1, 2, timesheet.Date, null)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(1, 2, timesheet.Date)).ReturnsAsync(false);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateTimesheetAsync(timesheet));
    }

    [Test]
    public void CreateTimesheetAsync_Throws_WhenProjectNotActive()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 8, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetTotalHoursForDateAsync(1, timesheet.Date)).ReturnsAsync(0);
        _unitOfWorkMock.Setup(u => u.Timesheets.HasDuplicateEntryAsync(1, 2, timesheet.Date, null)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(1, 2, timesheet.Date)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Inactive });
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CreateTimesheetAsync(timesheet));
    }

    [Test]
    public async Task UpdateTimesheetAsync_UpdatesFields()
    {
        var timesheet = new Timesheet { Id = 1, HoursWorked = 8, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today, Description = "desc" };
        var existing = new Timesheet { Id = 1, Status = TimesheetStatus.Draft, HoursWorked = 5, EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today, Description = "old" };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(existing);
        _unitOfWorkMock.Setup(u => u.Timesheets.GetTotalHoursForDateAsync(1, timesheet.Date)).ReturnsAsync(0);
        _unitOfWorkMock.Setup(u => u.Timesheets.HasDuplicateEntryAsync(1, 2, timesheet.Date, 1)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.ProjectAssignments.IsEmployeeAssignedToProjectAsync(1, 2, timesheet.Date)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.ProjectCodes.GetByIdAsync(2)).ReturnsAsync(new ProjectCode { Id = 2, Status = ProjectStatus.Active });
        var result = await _service.UpdateTimesheetAsync(timesheet);
        Assert.That(result.Description, Is.EqualTo("desc"));
        _unitOfWorkMock.Verify(u => u.Timesheets.UpdateAsync(existing), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public void UpdateTimesheetAsync_Throws_WhenNotFound()
    {
        var timesheet = new Timesheet { Id = 1 };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.UpdateTimesheetAsync(timesheet));
    }

    [Test]
    public void UpdateTimesheetAsync_Throws_WhenNotDraft()
    {
        var timesheet = new Timesheet { Id = 1 };
        var existing = new Timesheet { Id = 1, Status = TimesheetStatus.Submitted };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(existing);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateTimesheetAsync(timesheet));
    }

    [Test]
    public async Task SubmitTimesheetAsync_SetsStatusSubmitted()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Draft };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        var result = await _service.SubmitTimesheetAsync(1);
        Assert.That(result, Is.True);
        Assert.That(timesheet.Status, Is.EqualTo(TimesheetStatus.Submitted));
    }

    [Test]
    public void SubmitTimesheetAsync_Throws_WhenNotDraft()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Submitted };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.SubmitTimesheetAsync(1));
    }

    [Test]
    public async Task SubmitTimesheetAsync_ReturnsFalse_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        var result = await _service.SubmitTimesheetAsync(1);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteTimesheetAsync_DeletesDraft()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Draft };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        var result = await _service.DeleteTimesheetAsync(1);
        Assert.That(result, Is.True);
        _unitOfWorkMock.Verify(u => u.Timesheets.DeleteAsync(timesheet), Times.Once());
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Test]
    public void DeleteTimesheetAsync_Throws_WhenNotDraft()
    {
        var timesheet = new Timesheet { Id = 1, Status = TimesheetStatus.Submitted };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.DeleteTimesheetAsync(1));
    }

    [Test]
    public async Task DeleteTimesheetAsync_ReturnsFalse_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        var result = await _service.DeleteTimesheetAsync(1);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetEmployeeTimesheetsAsync_ReturnsList()
    {
        var timesheets = new List<Timesheet> { new Timesheet { Id = 1 } };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByEmployeeIdAsync(1)).ReturnsAsync(timesheets);
        var result = await _service.GetEmployeeTimesheetsAsync(1);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetTimesheetByIdAsync_ReturnsTimesheet()
    {
        var timesheet = new Timesheet { Id = 1 };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync(timesheet);
        var result = await _service.GetTimesheetByIdAsync(1);
        Assert.That(result, Is.EqualTo(timesheet));
    }

    [Test]
    public async Task GetTimesheetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        var result = await _service.GetTimesheetByIdAsync(1);
        Assert.That(result, Is.Null);
    }
}
