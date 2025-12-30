using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Services;

public interface ITimesheetApprovalService
{
    Task<bool> ApproveTimesheetAsync(int timesheetId, int managerId);
    Task<bool> RejectTimesheetAsync(int timesheetId, int managerId, string comments);
    Task<IEnumerable<Timesheet>> GetPendingTimesheetsAsync();
    Task<IEnumerable<Timesheet>> GetTimesheetsByStatusAsync(string status);
}
