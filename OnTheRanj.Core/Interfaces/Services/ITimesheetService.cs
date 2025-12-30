using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Services;

public interface ITimesheetService
{
    Task<Timesheet> CreateTimesheetAsync(Timesheet timesheet);
    Task<Timesheet> UpdateTimesheetAsync(Timesheet timesheet);
    Task<bool> SubmitTimesheetAsync(int timesheetId);
    Task<bool> DeleteTimesheetAsync(int timesheetId);
    Task<IEnumerable<Timesheet>> GetEmployeeTimesheetsAsync(int employeeId);
    Task<Timesheet?> GetTimesheetByIdAsync(int id);

    /// <summary>
    /// Gets all timesheets (for manager dashboard)
    /// </summary>
    Task<IEnumerable<Timesheet>> GetAllTimesheetsAsync();
}
