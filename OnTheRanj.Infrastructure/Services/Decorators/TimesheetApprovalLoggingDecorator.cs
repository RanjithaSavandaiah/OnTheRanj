using Microsoft.Extensions.Logging;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services.Decorators;

/// <summary>
/// Decorator Pattern: Adds logging to timesheet approval service
/// </summary>
public class TimesheetApprovalLoggingDecorator : ITimesheetApprovalService
{
    private readonly ITimesheetApprovalService _inner;
    private readonly ILogger<TimesheetApprovalLoggingDecorator> _logger;

    public TimesheetApprovalLoggingDecorator(ITimesheetApprovalService inner, ILogger<TimesheetApprovalLoggingDecorator> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<bool> ApproveTimesheetAsync(int timesheetId, int managerId)
    {
        _logger.LogInformation($"Manager {managerId} is approving timesheet {timesheetId}");
        var result = await _inner.ApproveTimesheetAsync(timesheetId, managerId);
        _logger.LogInformation($"Approval result: {result}");
        return result;
    }

    public async Task<bool> RejectTimesheetAsync(int timesheetId, int managerId, string comments)
    {
        _logger.LogInformation($"Manager {managerId} is rejecting timesheet {timesheetId} with comments: {comments}");
        var result = await _inner.RejectTimesheetAsync(timesheetId, managerId, comments);
        _logger.LogInformation($"Rejection result: {result}");
        return result;
    }

    public async Task<IEnumerable<Core.Entities.Timesheet>> GetPendingTimesheetsAsync()
    {
        return await _inner.GetPendingTimesheetsAsync();
    }

    public async Task<IEnumerable<Core.Entities.Timesheet>> GetTimesheetsByStatusAsync(string status)
    {
        return await _inner.GetTimesheetsByStatusAsync(status);
    }
}