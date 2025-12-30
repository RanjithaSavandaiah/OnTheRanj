using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Services;

public interface IProjectAssignmentService
{
    Task<ProjectAssignment> AssignProjectToEmployeeAsync(int employeeId, int projectCodeId, DateTime startDate, DateTime? endDate, int managerId);
    Task<bool> RemoveProjectAssignmentAsync(int assignmentId);
    Task<IEnumerable<ProjectAssignment>> GetEmployeeAssignmentsAsync(int employeeId);
    Task<IEnumerable<ProjectAssignment>> GetActiveEmployeeAssignmentsAsync(int employeeId);
    Task<bool> CanEmployeeSubmitTimesheetAsync(int employeeId, int projectCodeId, DateTime date);

    // Added for controller compatibility
    Task<ProjectAssignment?> UpdateProjectAssignmentAsync(ProjectAssignment assignment);
    Task<ProjectAssignment?> GetProjectAssignmentByIdAsync(int id);
    Task<IEnumerable<ProjectAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<ProjectAssignment>> GetAssignmentsByProjectCodeIdAsync(int projectCodeId);
    Task<IEnumerable<ProjectAssignment>> GetAllProjectAssignmentsAsync();
}
