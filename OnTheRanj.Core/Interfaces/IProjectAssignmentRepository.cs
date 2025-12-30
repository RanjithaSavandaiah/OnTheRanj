using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces;

/// <summary>
/// Specific repository interface for ProjectAssignment entity operations
/// </summary>
public interface IProjectAssignmentRepository : IRepository<ProjectAssignment>
{
    Task<IEnumerable<ProjectAssignment>> GetAllProjectAssignmentsAsync();
    Task<IEnumerable<ProjectAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<ProjectAssignment>> GetAssignmentsByProjectCodeIdAsync(int projectCodeId);
    Task<IEnumerable<ProjectAssignment>> GetActiveAssignmentsByEmployeeIdAsync(int employeeId);
    Task<bool> IsEmployeeAssignedToProjectAsync(int employeeId, int projectCodeId, DateTime date);
}
