using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Services.Implementations;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class ReportServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private ReportService _service;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new ReportService(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetDashboardSummaryAsync_ReturnsCorrectSummary()
    {
        // Arrange
        var timesheets = new List<Timesheet>
        {
            new Timesheet
            {
                EmployeeId = 1,
                    Employee = new User { FullName = "Alice Employee" },
                ProjectCodeId = 10,
                ProjectCode = new ProjectCode { Code = "P001", ProjectName = "Alpha", ClientName = "ClientA", IsBillable = true },
                HoursWorked = 8,
                Status = TimesheetStatus.Approved
            },
            new Timesheet
            {
                EmployeeId = 2,
                    Employee = new User { FullName = "Bob Employee" },
                ProjectCodeId = 11,
                ProjectCode = new ProjectCode { Code = "P002", ProjectName = "Beta", ClientName = "ClientB", IsBillable = false },
                HoursWorked = 6,
                Status = TimesheetStatus.Approved
            }
        };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(timesheets);

        // Act
        var result = await _service.GetDashboardSummaryAsync(DateTime.Now.AddDays(-7), DateTime.Now);

        // Assert
        Assert.That(result.Employees.Count, Is.EqualTo(2));
        Assert.That(result.Projects.Count, Is.EqualTo(2));
        Assert.That(result.BillableVsNonBillable.TotalBillableHours, Is.EqualTo(8));
        Assert.That(result.BillableVsNonBillable.TotalNonBillableHours, Is.EqualTo(6));
        Assert.That(result.BillableVsNonBillable.TotalHours, Is.EqualTo(14));
    }
    [Test]
    public async Task GetEmployeeHoursSummaryAsync_ReturnsSummary()
    {
        var timesheets = new List<Timesheet>
        {
            new Timesheet { EmployeeId = 1, Employee = new User { FullName = "A" }, ProjectCode = new ProjectCode { IsBillable = true }, HoursWorked = 8, Status = TimesheetStatus.Approved },
            new Timesheet { EmployeeId = 1, Employee = new User { FullName = "A" }, ProjectCode = new ProjectCode { IsBillable = false }, HoursWorked = 2, Status = TimesheetStatus.Approved },
            new Timesheet { EmployeeId = 2, Employee = new User { FullName = "B" }, ProjectCode = new ProjectCode { IsBillable = true }, HoursWorked = 5, Status = TimesheetStatus.Approved }
        };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(timesheets);
        var result = await _service.GetEmployeeHoursSummaryAsync(DateTime.Now.AddDays(-7), DateTime.Now);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetEmployeeHoursSummaryAsync_ReturnsEmpty_WhenNoApproved()
    {
        var timesheets = new List<Timesheet>();
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(timesheets);
        var result = await _service.GetEmployeeHoursSummaryAsync(DateTime.Now.AddDays(-7), DateTime.Now);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetProjectHoursSummaryAsync_ReturnsSummary()
    {
        var timesheets = new List<Timesheet>
        {
            new Timesheet { ProjectCodeId = 1, ProjectCode = new ProjectCode { Code = "P1", ProjectName = "Alpha", ClientName = "C1" }, EmployeeId = 1, HoursWorked = 8, Status = TimesheetStatus.Approved },
            new Timesheet { ProjectCodeId = 1, ProjectCode = new ProjectCode { Code = "P1", ProjectName = "Alpha", ClientName = "C1" }, EmployeeId = 2, HoursWorked = 4, Status = TimesheetStatus.Approved },
            new Timesheet { ProjectCodeId = 2, ProjectCode = new ProjectCode { Code = "P2", ProjectName = "Beta", ClientName = "C2" }, EmployeeId = 1, HoursWorked = 3, Status = TimesheetStatus.Approved }
        };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(timesheets);
        var result = await _service.GetProjectHoursSummaryAsync(DateTime.Now.AddDays(-7), DateTime.Now);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetProjectHoursSummaryAsync_ReturnsEmpty_WhenNoApproved()
    {
        var timesheets = new List<Timesheet>();
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(timesheets);
        var result = await _service.GetProjectHoursSummaryAsync(DateTime.Now.AddDays(-7), DateTime.Now);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetBillableVsNonBillableHoursAsync_ReturnsCorrectSummary()
    {
        var timesheets = new List<Timesheet>
        {
            new Timesheet { ProjectCode = new ProjectCode { IsBillable = true }, HoursWorked = 8, Status = TimesheetStatus.Approved },
            new Timesheet { ProjectCode = new ProjectCode { IsBillable = false }, HoursWorked = 2, Status = TimesheetStatus.Approved }
        };
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(timesheets);
        var result = await _service.GetBillableVsNonBillableHoursAsync(DateTime.Now.AddDays(-7), DateTime.Now);
        Assert.That(result.TotalBillableHours, Is.EqualTo(8));
        Assert.That(result.TotalNonBillableHours, Is.EqualTo(2));
        Assert.That(result.TotalHours, Is.EqualTo(10));
        Assert.That(result.BillablePercentage, Is.EqualTo(80));
    }

    [Test]
    public async Task GetBillableVsNonBillableHoursAsync_ReturnsZero_WhenNoApproved()
    {
        var timesheets = new List<Timesheet>();
        _unitOfWorkMock.Setup(u => u.Timesheets.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(timesheets);
        var result = await _service.GetBillableVsNonBillableHoursAsync(DateTime.Now.AddDays(-7), DateTime.Now);
        Assert.That(result.TotalBillableHours, Is.EqualTo(0));
        Assert.That(result.TotalNonBillableHours, Is.EqualTo(0));
        Assert.That(result.TotalHours, Is.EqualTo(0));
        Assert.That(result.BillablePercentage, Is.EqualTo(0));
    }
}
