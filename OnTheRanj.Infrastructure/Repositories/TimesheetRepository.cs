using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.Infrastructure.Repositories;

/// <summary>
/// Timesheet repository implementation with comprehensive querying
/// </summary>
public class TimesheetRepository : Repository<Timesheet>, ITimesheetRepository
{
    public TimesheetRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// Gets timesheets by employee ID
    /// </summary>
    public async Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Include(t => t.ProjectCode)
            .Include(t => t.Reviewer)
            .Where(t => t.EmployeeId == employeeId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Gets timesheets by project code ID
    /// </summary>
    public async Task<IEnumerable<Timesheet>> GetByProjectCodeIdAsync(int projectCodeId)
    {
        return await _dbSet
            .Include(t => t.Employee)
            .Include(t => t.ProjectCode)
            .Where(t => t.ProjectCodeId == projectCodeId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Gets timesheets by status
    /// </summary>
    public async Task<IEnumerable<Timesheet>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Include(t => t.Employee)
            .Include(t => t.ProjectCode)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Gets timesheets within a date range
    /// </summary>
    public async Task<IEnumerable<Timesheet>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.Employee)
            .Include(t => t.ProjectCode)
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Gets timesheets by employee and date range
    /// </summary>
    public async Task<IEnumerable<Timesheet>> GetByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.ProjectCode)
            .Where(t => t.EmployeeId == employeeId 
                && t.Date >= startDate 
                && t.Date <= endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Gets total hours logged for a specific date and employee
    /// </summary>
    public async Task<decimal> GetTotalHoursForDateAsync(int employeeId, DateTime date)
    {
        // Patch: Cast to double for SQLite compatibility, then back to decimal
        var sum = await _dbSet
            .Where(t => t.EmployeeId == employeeId && t.Date.Date == date.Date)
            .SumAsync(t => (double)t.HoursWorked);
        return (decimal)sum;
    }

    /// <summary>
    /// Checks for duplicate timesheet entry
    /// </summary>
    public async Task<bool> HasDuplicateEntryAsync(int employeeId, int projectCodeId, DateTime date, int? excludeTimesheetId = null)
    {
        var query = _dbSet.Where(t => t.EmployeeId == employeeId 
            && t.ProjectCodeId == projectCodeId 
            && t.Date.Date == date.Date);

        if (excludeTimesheetId.HasValue)
        {
            query = query.Where(t => t.Id != excludeTimesheetId.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Gets pending timesheets for manager review
    /// </summary>
    public async Task<IEnumerable<Timesheet>> GetPendingTimesheetsAsync()
    {
        return await _dbSet
            .Include(t => t.Employee)
            .Include(t => t.ProjectCode)
            .Where(t => t.Status == TimesheetStatus.Submitted)
            .OrderBy(t => t.Date)
            .ToListAsync();
    }
}
