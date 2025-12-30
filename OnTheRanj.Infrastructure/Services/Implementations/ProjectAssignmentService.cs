using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services.Implementations;

/// <summary>
/// Project assignment service implementation
/// </summary>
public class ProjectAssignmentService : IProjectAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectAssignmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IEnumerable<ProjectAssignment>> GetAllProjectAssignmentsAsync()
    {
        return await _unitOfWork.ProjectAssignments.GetAllProjectAssignmentsAsync();
    }

    public async Task<ProjectAssignment?> UpdateProjectAssignmentAsync(ProjectAssignment assignment)
    {
        var existing = await _unitOfWork.ProjectAssignments.GetByIdAsync(assignment.Id);
        if (existing == null) return null;
        existing.StartDate = assignment.StartDate;
        existing.EndDate = assignment.EndDate;
        existing.ProjectCodeId = assignment.ProjectCodeId;
        existing.EmployeeId = assignment.EmployeeId;
        await _unitOfWork.ProjectAssignments.UpdateAsync(existing);
        await _unitOfWork.CompleteAsync();
        return existing;
    }

    public async Task<ProjectAssignment?> GetProjectAssignmentByIdAsync(int id)
    {
        return await _unitOfWork.ProjectAssignments.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ProjectAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId)
    {
        return await _unitOfWork.ProjectAssignments.GetAssignmentsByEmployeeIdAsync(employeeId);
    }

    public async Task<IEnumerable<ProjectAssignment>> GetAssignmentsByProjectCodeIdAsync(int projectCodeId)
    {
        return await _unitOfWork.ProjectAssignments.GetAssignmentsByProjectCodeIdAsync(projectCodeId);
    }

    public async Task<ProjectAssignment> AssignProjectToEmployeeAsync(
        int employeeId, int projectCodeId, DateTime startDate, DateTime? endDate, int managerId)
    {
        // Validate employee exists
        var employee = await _unitOfWork.Users.GetByIdAsync(employeeId);
        if (employee == null || employee.Role != UserRole.Employee)
        {
            throw new InvalidOperationException("Invalid employee");
        }

        // Validate project exists and is active
        var projectCode = await _unitOfWork.ProjectCodes.GetByIdAsync(projectCodeId);
        if (projectCode == null || projectCode.Status != ProjectStatus.Active)
        {
            throw new InvalidOperationException("Invalid or inactive project code");
        }

        // Validate dates
        if (endDate.HasValue && endDate.Value < startDate)
        {
            throw new InvalidOperationException("End date cannot be before start date");
        }

        var assignment = new ProjectAssignment
        {
            EmployeeId = employeeId,
            ProjectCodeId = projectCodeId,
            StartDate = startDate,
            EndDate = endDate,
            CreatedBy = managerId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ProjectAssignments.AddAsync(assignment);
        await _unitOfWork.CompleteAsync();

        return assignment;
    }

    public async Task<bool> RemoveProjectAssignmentAsync(int assignmentId)
    {
        var assignment = await _unitOfWork.ProjectAssignments.GetByIdAsync(assignmentId);
        if (assignment == null) return false;

        await _unitOfWork.ProjectAssignments.DeleteAsync(assignment);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<IEnumerable<ProjectAssignment>> GetEmployeeAssignmentsAsync(int employeeId)
    {
        return await _unitOfWork.ProjectAssignments.GetAssignmentsByEmployeeIdAsync(employeeId);
    }

    public async Task<IEnumerable<ProjectAssignment>> GetActiveEmployeeAssignmentsAsync(int employeeId)
    {
        return await _unitOfWork.ProjectAssignments.GetActiveAssignmentsByEmployeeIdAsync(employeeId);
    }

    public async Task<bool> CanEmployeeSubmitTimesheetAsync(int employeeId, int projectCodeId, DateTime date)
    {
        // Check if assignment exists for the date
        var isAssigned = await _unitOfWork.ProjectAssignments
            .IsEmployeeAssignedToProjectAsync(employeeId, projectCodeId, date);
        
        if (!isAssigned) return false;

        // Check if project is active
        var project = await _unitOfWork.ProjectCodes.GetByIdAsync(projectCodeId);
        return project != null && project.Status == ProjectStatus.Active;
    }
}
