using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Infrastructure.Services.Strategies;

namespace OnTheRanj.Tests.Services;

[TestFixture]
public class DefaultApprovalStrategyTests
{
    private DefaultApprovalStrategy _strategy;

    [SetUp]
    public void Setup()
    {
        _strategy = new DefaultApprovalStrategy();
    }

    [Test]
    public void CanApprove_ReturnsTrue_WhenStatusIsSubmitted()
    {
        var timesheet = new Timesheet { Status = TimesheetStatus.Submitted };
        Assert.That(_strategy.CanApprove(timesheet), Is.True);
    }

    [Test]
    public void CanApprove_ReturnsFalse_WhenStatusIsNotSubmitted()
    {
        var timesheet = new Timesheet { Status = TimesheetStatus.Draft };
        Assert.That(_strategy.CanApprove(timesheet), Is.False);
    }

    [Test]
    public void CanReject_ReturnsTrue_WhenStatusIsSubmitted_AndCommentsNotEmpty()
    {
        var timesheet = new Timesheet { Status = TimesheetStatus.Submitted };
        Assert.That(_strategy.CanReject(timesheet, "Valid comment"), Is.True);
    }

    [Test]
    public void CanReject_ReturnsFalse_WhenStatusIsNotSubmitted()
    {
        var timesheet = new Timesheet { Status = TimesheetStatus.Draft };
        Assert.That(_strategy.CanReject(timesheet, "Valid comment"), Is.False);
    }

    [Test]
    public void CanReject_ReturnsFalse_WhenCommentsAreEmpty()
    {
        var timesheet = new Timesheet { Status = TimesheetStatus.Submitted };
        Assert.That(_strategy.CanReject(timesheet, " "), Is.False);
        Assert.That(_strategy.CanReject(timesheet, null), Is.False);
    }

    [Test]
    public void CanApprove_ReturnsFalse_WhenTimesheetIsNull()
    {
        Assert.That(_strategy.CanApprove(null), Is.False);
    }

    [Test]
    public void CanReject_ReturnsFalse_WhenTimesheetIsNull()
    {
        Assert.That(_strategy.CanReject(null, "Valid comment"), Is.False);
    }
}
