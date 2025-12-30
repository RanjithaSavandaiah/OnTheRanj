using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services.Implementations;

/// <summary>
/// Timesheet service implementation
/// Implements Strategy pattern for different validation strategies
/// </summary>
public class TimesheetService : ITimesheetService
{   
    private readonly IUnitOfWork _unitOfWork;

    public TimesheetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IEnumerable<Timesheet>> GetAllTimesheetsAsync()
    {
        return await _unitOfWork.Timesheets.GetAllAsync();
    }

    public async Task<Timesheet> CreateTimesheetAsync(Timesheet timesheet)
    {
        await ValidateTimesheetAsync(timesheet);

        timesheet.Status = TimesheetStatus.Draft;
        timesheet.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Timesheets.AddAsync(timesheet);
        await _unitOfWork.CompleteAsync();

        return timesheet;
    }

    public async Task<Timesheet> UpdateTimesheetAsync(Timesheet timesheet)
    {
        var existing = await _unitOfWork.Timesheets.GetByIdAsync(timesheet.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Timesheet with ID {timesheet.Id} not found");
        }

        if (existing.Status != TimesheetStatus.Draft)
        {
            throw new InvalidOperationException("Only draft timesheets can be updated");
        }

        await ValidateTimesheetAsync(timesheet, existing.Id);

        existing.ProjectCodeId = timesheet.ProjectCodeId;
        existing.Date = timesheet.Date;
        existing.HoursWorked = timesheet.HoursWorked;
        existing.Description = timesheet.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Timesheets.UpdateAsync(existing);
        await _unitOfWork.CompleteAsync();

        return existing;
    }

    public async Task<bool> SubmitTimesheetAsync(int timesheetId)
    {
        var timesheet = await _unitOfWork.Timesheets.GetByIdAsync(timesheetId);
        if (timesheet == null) return false;

        if (timesheet.Status != TimesheetStatus.Draft)
        {
            throw new InvalidOperationException("Only draft timesheets can be submitted");
        }

        timesheet.Status = TimesheetStatus.Submitted;
        timesheet.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Timesheets.UpdateAsync(timesheet);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> DeleteTimesheetAsync(int timesheetId)
    {
        var timesheet = await _unitOfWork.Timesheets.GetByIdAsync(timesheetId);
        if (timesheet == null) return false;

        if (timesheet.Status != TimesheetStatus.Draft)
        {
            throw new InvalidOperationException("Only draft timesheets can be deleted");
        }

        await _unitOfWork.Timesheets.DeleteAsync(timesheet);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<IEnumerable<Timesheet>> GetEmployeeTimesheetsAsync(int employeeId)
    {
        return await _unitOfWork.Timesheets.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<Timesheet?> GetTimesheetByIdAsync(int id)
    {
        return await _unitOfWork.Timesheets.GetByIdAsync(id);
    }

    private async Task ValidateTimesheetAsync(Timesheet timesheet, int? excludeId = null)
    {
        // Validate hours (max 24 per day)
        if (timesheet.HoursWorked <= 0 || timesheet.HoursWorked > 24)
        {
            throw new InvalidOperationException("Hours worked must be between 0 and 24");
        }

        // Check total hours for the day
        var totalHours = await _unitOfWork.Timesheets.GetTotalHoursForDateAsync(
            timesheet.EmployeeId, timesheet.Date);
        
        if (excludeId.HasValue)
        {
            var existing = await _unitOfWork.Timesheets.GetByIdAsync(excludeId.Value);
            if (existing != null)
            {
                totalHours -= existing.HoursWorked;
            }
        }

        if (totalHours + timesheet.HoursWorked > 24)
        {
            throw new InvalidOperationException($"Total hours for {timesheet.Date:yyyy-MM-dd} would exceed 24 hours");
        }

        // Check for duplicate entry
        var hasDuplicate = await _unitOfWork.Timesheets.HasDuplicateEntryAsync(
            timesheet.EmployeeId, timesheet.ProjectCodeId, timesheet.Date, excludeId);
        
        if (hasDuplicate)
        {
            throw new InvalidOperationException("A timesheet entry already exists for this project and date");
        }

        // Validate employee is assigned to project
        var isAssigned = await _unitOfWork.ProjectAssignments
            .IsEmployeeAssignedToProjectAsync(timesheet.EmployeeId, timesheet.ProjectCodeId, timesheet.Date);
        
        if (!isAssigned)
        {
            throw new InvalidOperationException("Employee is not assigned to this project for the specified date");
        }

        // Validate project is active
        var project = await _unitOfWork.ProjectCodes.GetByIdAsync(timesheet.ProjectCodeId);
        if (project == null || project.Status != ProjectStatus.Active)
        {
            throw new InvalidOperationException("Project is not active");
        }
    }
}
