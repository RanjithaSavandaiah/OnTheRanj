using Microsoft.AspNetCore.Mvc;
using Moq;
using OnTheRanj.API.Controllers;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Tests.Controllers;

[TestFixture]
public class TimesheetApprovalsControllerTests
{
    private Mock<ITimesheetApprovalService> _serviceMock;
    private TimesheetApprovalsController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<ITimesheetApprovalService>();
        _controller = new TimesheetApprovalsController(_serviceMock.Object);
    }

    [Test]
    public async Task GetPendingTimesheets_ReturnsOk()
    {
        var timesheets = new List<Timesheet> { new Timesheet { Id = 1, EmployeeId = 1, ProjectCodeId = 1, Date = System.DateTime.Today, HoursWorked = 8, Status = TimesheetStatus.Submitted, Employee = new User { FullName = "Test" }, ProjectCode = new ProjectCode { ProjectName = "P" } } };
        _serviceMock.Setup(s => s.GetPendingTimesheetsAsync()).ReturnsAsync(timesheets);
        var result = await _controller.GetPendingTimesheets();
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var dtos = (IEnumerable<TimesheetApprovalDto>)(okResult!.Value!);
        Assert.That(dtos.First().EmployeeName, Is.EqualTo("Test"));
    }

    [Test]
    public async Task GetTimesheetsByStatus_ReturnsOk()
    {
        var timesheets = new List<Timesheet> { new Timesheet { Id = 1, EmployeeId = 1, ProjectCodeId = 1, Date = System.DateTime.Today, HoursWorked = 8, Status = TimesheetStatus.Submitted, Employee = new User { FullName = "Test" }, ProjectCode = new ProjectCode { ProjectName = "P" } } };
        _serviceMock.Setup(s => s.GetTimesheetsByStatusAsync("Submitted")).ReturnsAsync(timesheets);
        var result = await _controller.GetTimesheetsByStatus("Submitted");
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var dtos = (IEnumerable<TimesheetApprovalDto>)(okResult!.Value!);
        Assert.That(dtos.First().EmployeeName, Is.EqualTo("Test"));
    }

    [Test]
    public async Task ApproveTimesheet_ReturnsOk_WhenSuccess()
    {
        _serviceMock.Setup(s => s.ApproveTimesheetAsync(1, It.IsAny<int>())).ReturnsAsync(true);
        var result = await _controller.ApproveTimesheet(1);
        Assert.That(result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task ApproveTimesheet_ReturnsBadRequest_WhenFail()
    {
        _serviceMock.Setup(s => s.ApproveTimesheetAsync(1, It.IsAny<int>())).ReturnsAsync(false);
        var result = await _controller.ApproveTimesheet(1);
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task RejectTimesheet_ReturnsOk_WhenSuccess()
    {
        _serviceMock.Setup(s => s.RejectTimesheetAsync(1, It.IsAny<int>(), "reason")).ReturnsAsync(true);
        var req = new RejectTimesheetRequest { Comments = "reason" };
        var result = await _controller.RejectTimesheet(1, req);
        Assert.That(result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task RejectTimesheet_ReturnsBadRequest_WhenNoComments()
    {
        var req = new RejectTimesheetRequest { Comments = null };
        var result = await _controller.RejectTimesheet(1, req);
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task RejectTimesheet_ReturnsBadRequest_WhenFail()
    {
        _serviceMock.Setup(s => s.RejectTimesheetAsync(1, It.IsAny<int>(), "reason")).ReturnsAsync(false);
        var req = new RejectTimesheetRequest { Comments = "reason" };
        var result = await _controller.RejectTimesheet(1, req);
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }
}
