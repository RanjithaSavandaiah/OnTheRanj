using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services.Implementations;

/// <summary>
/// Timesheet approval service implementation
/// </summary>
/// <summary>
/// Implements Strategy Pattern for approval rules and is designed for Decorator Pattern extension.
/// </summary>
public class TimesheetApprovalService : ITimesheetApprovalService
{
    private readonly IUnitOfWork _unitOfWork;
    // Strategy Pattern: Approval rules are injected
    private readonly Core.Interfaces.Strategies.ITimesheetApprovalStrategy _approvalStrategy;

    public TimesheetApprovalService(IUnitOfWork unitOfWork, Core.Interfaces.Strategies.ITimesheetApprovalStrategy approvalStrategy)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _approvalStrategy = approvalStrategy ?? throw new ArgumentNullException(nameof(approvalStrategy));
    }

    /// <summary>
    /// Uses Strategy Pattern for approval rules
    /// </summary>
    public async Task<bool> ApproveTimesheetAsync(int timesheetId, int managerId)
    {
        var timesheet = await _unitOfWork.Timesheets.GetByIdAsync(timesheetId);
        if (timesheet == null) return false;

        if (!_approvalStrategy.CanApprove(timesheet))
        {
            throw new InvalidOperationException("Only submitted timesheets can be approved (Strategy Pattern)");
        }

        timesheet.Status = TimesheetStatus.Approved;
        timesheet.ReviewedBy = managerId;
        timesheet.ReviewedAt = DateTime.UtcNow;
        timesheet.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Timesheets.UpdateAsync(timesheet);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    /// <summary>
    /// Uses Strategy Pattern for rejection rules
    /// </summary>
    public async Task<bool> RejectTimesheetAsync(int timesheetId, int managerId, string comments)
    {
        var timesheet = await _unitOfWork.Timesheets.GetByIdAsync(timesheetId);
        if (timesheet == null) return false;

        if (!_approvalStrategy.CanReject(timesheet, comments))
        {
            throw new InvalidOperationException("Only submitted timesheets with comments can be rejected (Strategy Pattern)");
        }

        timesheet.Status = TimesheetStatus.Rejected;
        timesheet.ReviewedBy = managerId;
        timesheet.ReviewedAt = DateTime.UtcNow;
        timesheet.ManagerComments = comments;
        timesheet.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Timesheets.UpdateAsync(timesheet);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<IEnumerable<Timesheet>> GetPendingTimesheetsAsync()
    {
        return await _unitOfWork.Timesheets.Query()
            .Where(ts => ts.Status == TimesheetStatus.Submitted || ts.Status == TimesheetStatus.Pending)
            .Include(ts => ts.Employee)
            .Include(ts => ts.ProjectCode)
            .ToListAsync();
    }

    public async Task<IEnumerable<Timesheet>> GetTimesheetsByStatusAsync(string status)
    {
        return await _unitOfWork.Timesheets.Query()
            .Where(ts => ts.Status == status)
            .Include(ts => ts.Employee)
            .Include(ts => ts.ProjectCode)
            .ToListAsync();
    }
}
