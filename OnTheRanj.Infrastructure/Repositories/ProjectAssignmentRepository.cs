using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.Infrastructure.Repositories;

/// <summary>
/// ProjectAssignment repository implementation
/// </summary>
public class ProjectAssignmentRepository : Repository<ProjectAssignment>, IProjectAssignmentRepository
{
    public ProjectAssignmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ProjectAssignment>> GetAllProjectAssignmentsAsync()
    {
        return await _dbSet
            .Include(pa => pa.ProjectCode)
            .Include(pa => pa.Employee)
            .OrderByDescending(pa => pa.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Gets active assignments for an employee
    /// </summary>
    public async Task<IEnumerable<ProjectAssignment>> GetActiveAssignmentsByEmployeeIdAsync(int employeeId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Include(pa => pa.ProjectCode)
            .Include(pa => pa.Employee)
            .Where(pa => pa.EmployeeId == employeeId
                && pa.StartDate <= today
                && (pa.EndDate == null || pa.EndDate >= today)
                && pa.ProjectCode.Status == ProjectStatus.Active)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all assignments for an employee
    /// </summary>
    public async Task<IEnumerable<ProjectAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Include(pa => pa.ProjectCode)
            .Include(pa => pa.Employee)
            .Where(pa => pa.EmployeeId == employeeId)
            .OrderByDescending(pa => pa.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Gets assignments for a project code
    /// </summary>
    public async Task<IEnumerable<ProjectAssignment>> GetAssignmentsByProjectCodeIdAsync(int projectCodeId)
    {
        return await _dbSet
            .Include(pa => pa.Employee)
            .Include(pa => pa.ProjectCode)
            .Where(pa => pa.ProjectCodeId == projectCodeId)
            .OrderByDescending(pa => pa.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Checks if employee is assigned to a project code on a specific date
    /// </summary>
    public async Task<bool> IsEmployeeAssignedToProjectAsync(int employeeId, int projectCodeId, DateTime date)
    {
        var assignments = await _dbSet
            .Where(pa => pa.EmployeeId == employeeId && pa.ProjectCodeId == projectCodeId)
            .ToListAsync();
        
        var result = assignments.Any(pa => pa.StartDate <= date && (pa.EndDate == null || pa.EndDate >= date));
        return result;
    }
}
