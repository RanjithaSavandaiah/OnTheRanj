using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces;

public interface ITimesheetRepository : IRepository<Timesheet>
{
    IQueryable<Timesheet> Query();

    Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Timesheet>> GetByProjectCodeIdAsync(int projectCodeId);
    Task<IEnumerable<Timesheet>> GetByStatusAsync(string status);
    Task<IEnumerable<Timesheet>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Timesheet>> GetByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalHoursForDateAsync(int employeeId, DateTime date);
    Task<bool> HasDuplicateEntryAsync(int employeeId, int projectCodeId, DateTime date, int? excludeTimesheetId = null);
    Task<IEnumerable<Timesheet>> GetPendingTimesheetsAsync();
}
