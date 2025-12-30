using Microsoft.AspNetCore.Mvc;
using Moq;
using OnTheRanj.API.Controllers;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Tests.Controllers;


[TestFixture]
public class TimesheetsControllerTests
{
    private Mock<ITimesheetService> _serviceMock;
    private TimesheetsController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<ITimesheetService>();
        _controller = new TimesheetsController(_serviceMock.Object);
    }

    [Test]
    public async Task SubmitWeek_AddsInvalidOperationExceptionResult()
    {
        var request = new TimesheetWeekSubmitRequest
        {
            EmployeeId = 1,
            Entries = new List<TimesheetWeekEntryDto>
            {
                new TimesheetWeekEntryDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" }
            }
        };
        _serviceMock.Setup(s => s.CreateTimesheetAsync(It.IsAny<Timesheet>())).ThrowsAsync(new InvalidOperationException("bad op"));
        var result = await _controller.SubmitWeek(request) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Null);
        var value = result!.Value!;
        var resultsProp = value.GetType().GetProperty("results");
        Assert.That(resultsProp, Is.Not.Null, "Property 'results' not found");
        var resultsList = resultsProp.GetValue(value) as System.Collections.IEnumerable;
        Assert.That(resultsList, Is.Not.Null);
        var enumerator = resultsList.GetEnumerator();
        Assert.That(enumerator.MoveNext(), Is.True);
        var firstResult = enumerator.Current;
        var successProp = firstResult.GetType().GetProperty("success");
        var messageProp = firstResult.GetType().GetProperty("message");
        Assert.That(successProp, Is.Not.Null);
        Assert.That(messageProp, Is.Not.Null);
        Assert.That((bool)successProp.GetValue(firstResult), Is.False);
        Assert.That((string)messageProp.GetValue(firstResult), Is.EqualTo("bad op"));
    }

    [Test]
    public async Task SubmitWeek_AddsGenericExceptionResult()
    {
        var request = new TimesheetWeekSubmitRequest
        {
            EmployeeId = 1,
            Entries = new List<TimesheetWeekEntryDto>
            {
                new TimesheetWeekEntryDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" }
            }
        };
        _serviceMock.Setup(s => s.CreateTimesheetAsync(It.IsAny<Timesheet>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.SubmitWeek(request) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Null);
        var value = result!.Value!;
        var resultsProp = value.GetType().GetProperty("results");
        Assert.That(resultsProp, Is.Not.Null, "Property 'results' not found");
        var resultsList = resultsProp.GetValue(value) as System.Collections.IEnumerable;
        Assert.That(resultsList, Is.Not.Null);
        var enumerator = resultsList.GetEnumerator();
        Assert.That(enumerator.MoveNext(), Is.True);
        var firstResult = enumerator.Current;
        var successProp = firstResult.GetType().GetProperty("success");
        var messageProp = firstResult.GetType().GetProperty("message");
        var detailProp = firstResult.GetType().GetProperty("detail");
        Assert.That(successProp, Is.Not.Null);
        Assert.That(messageProp, Is.Not.Null);
        Assert.That(detailProp, Is.Not.Null);
        Assert.That((bool)successProp.GetValue(firstResult), Is.False);
        Assert.That((string)messageProp.GetValue(firstResult), Is.EqualTo("Internal server error."));
        Assert.That((string)detailProp.GetValue(firstResult), Is.EqualTo("fail"));
    }

    [Test]
    public async Task SubmitTimesheet_ReturnsBadRequest_OnInvalidOperationException()
    {
        _serviceMock.Setup(s => s.SubmitTimesheetAsync(1)).ThrowsAsync(new InvalidOperationException("bad op"));
        var actionResult = await _controller.SubmitTimesheet(1);
        Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task SubmitTimesheet_ReturnsStatus500_OnException()
    {
        _serviceMock.Setup(s => s.SubmitTimesheetAsync(1)).ThrowsAsync(new Exception("fail"));
        var actionResult = await _controller.SubmitTimesheet(1);
        Assert.That(actionResult.Result, Is.TypeOf<ObjectResult>());
        var objResult = actionResult.Result as ObjectResult;
        Assert.That(objResult!.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task CreateTimesheet_ReturnsBadRequest_OnInvalidOperationException()
    {
        var dto = new TimesheetDto { EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        _serviceMock.Setup(s => s.CreateTimesheetAsync(It.IsAny<Timesheet>())).ThrowsAsync(new InvalidOperationException("bad op"));
        var actionResult = await _controller.CreateTimesheet(dto);
        Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateTimesheet_ReturnsStatus500_OnException()
    {
        var dto = new TimesheetDto { EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        _serviceMock.Setup(s => s.CreateTimesheetAsync(It.IsAny<Timesheet>())).ThrowsAsync(new Exception("fail"));
        var actionResult = await _controller.CreateTimesheet(dto);
        Assert.That(actionResult.Result, Is.TypeOf<ObjectResult>());
        var objResult = actionResult.Result as ObjectResult;
        Assert.That(objResult!.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task UpdateTimesheet_ReturnsBadRequest_OnInvalidOperationException()
    {
        var dto = new TimesheetDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        var existing = new Timesheet { Id = 1, ProjectCode = new ProjectCode { ProjectName = "P" } };
        _serviceMock.Setup(s => s.GetTimesheetByIdAsync(1)).ReturnsAsync(existing);
        _serviceMock.Setup(s => s.UpdateTimesheetAsync(existing)).ThrowsAsync(new InvalidOperationException("bad op"));
        var actionResult = await _controller.UpdateTimesheet(1, dto);
        Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateTimesheet_ReturnsStatus500_OnException()
    {
        var dto = new TimesheetDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        var existing = new Timesheet { Id = 1, ProjectCode = new ProjectCode { ProjectName = "P" } };
        _serviceMock.Setup(s => s.GetTimesheetByIdAsync(1)).ReturnsAsync(existing);
        _serviceMock.Setup(s => s.UpdateTimesheetAsync(existing)).ThrowsAsync(new Exception("fail"));
        var actionResult = await _controller.UpdateTimesheet(1, dto);
        Assert.That(actionResult.Result, Is.TypeOf<ObjectResult>());
        var objResult = actionResult.Result as ObjectResult;
        Assert.That(objResult!.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task SubmitWeek_ReturnsOk_WithResults()
    {
        var request = new TimesheetWeekSubmitRequest
        {
            EmployeeId = 1,
            Entries = new List<TimesheetWeekEntryDto>
            {
                new TimesheetWeekEntryDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" }
            }
        };
        _serviceMock.Setup(s => s.CreateTimesheetAsync(It.IsAny<Timesheet>())).ReturnsAsync(new Timesheet { Id = 1, Status = TimesheetStatus.Draft });
        var result = await _controller.SubmitWeek(request) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.Not.Null);
    }

    [Test]
    public async Task SubmitWeek_ReturnsBadRequest_WhenNoEntries()
    {
        var request = new TimesheetWeekSubmitRequest { EmployeeId = 1, Entries = new List<TimesheetWeekEntryDto>() };
        var result = await _controller.SubmitWeek(request) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task SubmitTimesheet_ReturnsOk_WhenSuccess()
    {
        _serviceMock.Setup(s => s.SubmitTimesheetAsync(1)).ReturnsAsync(true);
        _serviceMock.Setup(s => s.GetTimesheetByIdAsync(1)).ReturnsAsync(new Timesheet { Id = 1, Status = TimesheetStatus.Submitted, ProjectCode = new ProjectCode { ProjectName = "P" } });
        var actionResult = await _controller.SubmitTimesheet(1);
        Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
        var okResult = actionResult.Result as OkObjectResult;
        Assert.That(okResult!.Value, Is.Not.Null);
    }

    [Test]
    public async Task SubmitTimesheet_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.SubmitTimesheetAsync(1)).ReturnsAsync(false);
        var actionResult = await _controller.SubmitTimesheet(1);
        Assert.That(actionResult.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetAllTimesheets_ReturnsOk()
    {
        _serviceMock.Setup(s => s.GetAllTimesheetsAsync()).ReturnsAsync(new List<Timesheet> { new Timesheet { Id = 1, ProjectCode = new ProjectCode { ProjectName = "P" } } });
        var actionResult = await _controller.GetAllTimesheets();
        Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetPendingTimesheets_ReturnsOk()
    {
        _serviceMock.Setup(s => s.GetAllTimesheetsAsync()).ReturnsAsync(new List<Timesheet> { new Timesheet { Id = 1, Status = TimesheetStatus.Submitted, ProjectCode = new ProjectCode { ProjectName = "P" } } });
        var actionResult = await _controller.GetPendingTimesheets();
        Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetEmployeeTimesheets_ReturnsOk()
    {
        _serviceMock.Setup(s => s.GetEmployeeTimesheetsAsync(1)).ReturnsAsync(new List<Timesheet> { new Timesheet { Id = 1, ProjectCode = new ProjectCode { ProjectName = "P" } } });
        var actionResult = await _controller.GetEmployeeTimesheets(1);
        Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task CreateTimesheet_ReturnsCreatedAtAction()
    {
        var dto = new TimesheetDto { EmployeeId = 1, ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        _serviceMock.Setup(s => s.CreateTimesheetAsync(It.IsAny<Timesheet>())).ReturnsAsync(new Timesheet { Id = 1, ProjectCode = new ProjectCode { ProjectName = "P" } });
        var actionResult = await _controller.CreateTimesheet(dto);
        Assert.That(actionResult.Result, Is.TypeOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task UpdateTimesheet_ReturnsOk_WhenSuccess()
    {
        var dto = new TimesheetDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        var existing = new Timesheet { Id = 1, ProjectCode = new ProjectCode { ProjectName = "P" } };
        _serviceMock.Setup(s => s.GetTimesheetByIdAsync(1)).ReturnsAsync(existing);
        _serviceMock.Setup(s => s.UpdateTimesheetAsync(existing)).ReturnsAsync(existing);
        var actionResult = await _controller.UpdateTimesheet(1, dto);
        Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
        var okResult = actionResult.Result as OkObjectResult;
        Assert.That(okResult!.Value, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTimesheet_ReturnsNotFound_WhenNotFound()
    {
        var dto = new TimesheetDto { ProjectCodeId = 2, Date = DateTime.Today, HoursWorked = 8, Description = "Work" };
        _serviceMock.Setup(s => s.GetTimesheetByIdAsync(1)).ReturnsAsync((Timesheet?)null);
        var actionResult = await _controller.UpdateTimesheet(1, dto);
        Assert.That(actionResult.Result, Is.TypeOf<NotFoundResult>());
    }
}
