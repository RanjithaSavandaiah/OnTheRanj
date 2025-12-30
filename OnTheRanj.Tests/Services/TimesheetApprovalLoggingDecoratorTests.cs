using Microsoft.Extensions.Logging;
using Moq;
using OnTheRanj.Core.Entities;
using OnTheRanj.Infrastructure.Services.Decorators;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class TimesheetApprovalLoggingDecoratorTests
{
    private Mock<ITimesheetApprovalService> _innerMock;
    private Mock<ILogger<TimesheetApprovalLoggingDecorator>> _loggerMock;
    private TimesheetApprovalLoggingDecorator _decorator;

    [SetUp]
    public void Setup()
    {
        _innerMock = new Mock<ITimesheetApprovalService>();
        _loggerMock = new Mock<ILogger<TimesheetApprovalLoggingDecorator>>();
        _decorator = new TimesheetApprovalLoggingDecorator(_innerMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task ApproveTimesheetAsync_LogsAndDelegates()
    {
        _innerMock.Setup(x => x.ApproveTimesheetAsync(1, 2)).ReturnsAsync(true);
        var result = await _decorator.ApproveTimesheetAsync(1, 2);
        Assert.That(result, Is.True);
        _innerMock.Verify(x => x.ApproveTimesheetAsync(1, 2), Times.Once);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("approving timesheet")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Approval result")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Test]
    public async Task RejectTimesheetAsync_LogsAndDelegates()
    {
        _innerMock.Setup(x => x.RejectTimesheetAsync(1, 2, "bad"))
            .ReturnsAsync(false);
        var result = await _decorator.RejectTimesheetAsync(1, 2, "bad");
        Assert.That(result, Is.False);
        _innerMock.Verify(x => x.RejectTimesheetAsync(1, 2, "bad"), Times.Once);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("rejecting timesheet")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Rejection result")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Test]
    public async Task GetPendingTimesheetsAsync_Delegates()
    {
        var expected = new List<Timesheet> { new Timesheet { Id = 1 } };
        _innerMock.Setup(x => x.GetPendingTimesheetsAsync()).ReturnsAsync(expected);
        var result = await _decorator.GetPendingTimesheetsAsync();
        Assert.That(result, Is.EqualTo(expected));
        _innerMock.Verify(x => x.GetPendingTimesheetsAsync(), Times.Once);
    }

    [Test]
    public async Task GetTimesheetsByStatusAsync_Delegates()
    {
        var expected = new List<Timesheet> { new Timesheet { Id = 2 } };
        _innerMock.Setup(x => x.GetTimesheetsByStatusAsync("Submitted")).ReturnsAsync(expected);
        var result = await _decorator.GetTimesheetsByStatusAsync("Submitted");
        Assert.That(result, Is.EqualTo(expected));
        _innerMock.Verify(x => x.GetTimesheetsByStatusAsync("Submitted"), Times.Once);
    }
}
