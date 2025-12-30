using Microsoft.AspNetCore.Mvc;
using Moq;
using OnTheRanj.API.Controllers;
using OnTheRanj.Core.Interfaces.Services;
using OnTheRanj.Core.DTOs;

namespace OnTheRanj.Tests.Controllers;

[TestFixture]
public class ReportsControllerTests
{
    private Mock<IReportService> _serviceMock;
    private ReportsController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IReportService>();
        _controller = new ReportsController(_serviceMock.Object);
    }

    [Test]
    public async Task GetDashboardSummary_Returns500_OnException()
    {
        _serviceMock.Setup(s => s.GetDashboardSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.GetDashboardSummary(DateTime.Now, DateTime.Now) as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result.Value, Is.EqualTo("fail"));
    }

    [Test]
    public async Task GetEmployeeHoursSummary_Returns500_OnException()
    {
        _serviceMock.Setup(s => s.GetEmployeeHoursSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.GetEmployeeHoursSummary(DateTime.Now, DateTime.Now) as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result.Value, Is.EqualTo("fail"));
    }

    [Test]
    public async Task GetProjectHoursSummary_Returns500_OnException()
    {
        _serviceMock.Setup(s => s.GetProjectHoursSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.GetProjectHoursSummary(DateTime.Now, DateTime.Now) as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result.Value, Is.EqualTo("fail"));
    }

    [Test]
    public async Task GetBillableVsNonBillableHours_Returns500_OnException()
    {
        _serviceMock.Setup(s => s.GetBillableVsNonBillableHoursAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.GetBillableVsNonBillableHours(DateTime.Now, DateTime.Now) as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result.Value, Is.EqualTo("fail"));
    }

    [Test]
    public async Task GetDashboardSummary_ReturnsOk()
    {
        var summary = new DashboardSummaryDto();
        _serviceMock.Setup(s => s.GetDashboardSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(summary);
        var result = await _controller.GetDashboardSummary(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(summary));
    }

    [Test]
    public async Task GetEmployeeHoursSummary_ReturnsOk()
    {
        var summary = new List<EmployeeHoursSummary> { new EmployeeHoursSummary { EmployeeId = 1, EmployeeName = "Test", TotalHours = 10, BillableHours = 8, NonBillableHours = 2 } };
        _serviceMock.Setup(s => s.GetEmployeeHoursSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(summary);
        var result = await _controller.GetEmployeeHoursSummary(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(summary));
    }

    [Test]
    public async Task GetEmployeeHoursSummary_ReturnsOk_WhenEmpty()
    {
        _serviceMock.Setup(s => s.GetEmployeeHoursSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<EmployeeHoursSummary>());
        var result = await _controller.GetEmployeeHoursSummary(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.Not.Null);
        Assert.That(result.Value, Is.InstanceOf<List<EmployeeHoursSummary>>());
        Assert.That(((List<EmployeeHoursSummary>)result.Value!).Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetProjectHoursSummary_ReturnsOk()
    {
        var summary = new List<ProjectHoursSummary> { new ProjectHoursSummary { ProjectCodeId = 1, ProjectCode = "P1", ProjectName = "Project 1", ClientName = "Client", TotalHours = 10, EmployeeCount = 2 } };
        _serviceMock.Setup(s => s.GetProjectHoursSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(summary);
        var result = await _controller.GetProjectHoursSummary(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(summary));
    }

    [Test]
    public async Task GetProjectHoursSummary_ReturnsOk_WhenEmpty()
    {
        _serviceMock.Setup(s => s.GetProjectHoursSummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<ProjectHoursSummary>());
        var result = await _controller.GetProjectHoursSummary(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.Not.Null);
        Assert.That(result.Value, Is.InstanceOf<List<ProjectHoursSummary>>());
        Assert.That(((List<ProjectHoursSummary>)result.Value!).Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetBillableVsNonBillableHours_ReturnsOk()
    {
        var summary = new BillableHoursSummary { TotalBillableHours = 5, TotalNonBillableHours = 5, TotalHours = 10, BillablePercentage = 50 };
        _serviceMock.Setup(s => s.GetBillableVsNonBillableHoursAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(summary);
        var result = await _controller.GetBillableVsNonBillableHours(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(summary));
    }

    [Test]
    public async Task GetBillableVsNonBillableHours_ReturnsOk_WhenNull()
    {
        _serviceMock.Setup(s => s.GetBillableVsNonBillableHoursAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync((BillableHoursSummary)null!);
        var result = await _controller.GetBillableVsNonBillableHours(DateTime.Now, DateTime.Now) as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.Null);
    }
}

