using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces.Strategies;

namespace OnTheRanj.Infrastructure.Services.Strategies;

/// <summary>
/// Strategy Pattern: Default approval strategy for timesheets
/// </summary>
public class DefaultApprovalStrategy : ITimesheetApprovalStrategy
{
    public bool CanApprove(Timesheet timesheet)
    {
        if (timesheet == null) return false;
        return timesheet.Status == TimesheetStatus.Submitted;
    }

    public bool CanReject(Timesheet timesheet, string comments)
    {
        if (timesheet == null) return false;
        return timesheet.Status == TimesheetStatus.Submitted && !string.IsNullOrWhiteSpace(comments);
    }
}
