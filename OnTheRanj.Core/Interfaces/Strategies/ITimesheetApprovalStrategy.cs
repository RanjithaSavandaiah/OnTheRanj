using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Strategies;

/// <summary>
/// Strategy Pattern: Interface for timesheet approval rules
/// </summary>
public interface ITimesheetApprovalStrategy
{
    bool CanApprove(Timesheet timesheet);
    bool CanReject(Timesheet timesheet, string comments);
}
