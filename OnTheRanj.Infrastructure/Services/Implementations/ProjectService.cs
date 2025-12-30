using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services.Implementations;

/// <summary>
/// Project management service implementation
/// Handles all project code related operations
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ProjectCode> CreateProjectCodeAsync(ProjectCode projectCode, int managerId)
    {
        // Validate unique code
        var existing = await _unitOfWork.ProjectCodes.GetByCodeAsync(projectCode.Code);
        if (existing != null)
        {
            throw new InvalidOperationException($"Project code '{projectCode.Code}' already exists");
        }

        projectCode.CreatedBy = managerId;
        projectCode.CreatedAt = DateTime.UtcNow;
        projectCode.Status = ProjectStatus.Active;

        await _unitOfWork.ProjectCodes.AddAsync(projectCode);
        await _unitOfWork.CompleteAsync();

        return projectCode;
    }

    public async Task<ProjectCode> UpdateProjectCodeAsync(ProjectCode projectCode)
    {
        var existing = await _unitOfWork.ProjectCodes.GetByIdAsync(projectCode.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Project code with ID {projectCode.Id} not found");
        }

        // Check if code is being changed and if it's unique
        if (existing.Code != projectCode.Code)
        {
            var codeExists = await _unitOfWork.ProjectCodes.GetByCodeAsync(projectCode.Code);
            if (codeExists != null)
            {
                throw new InvalidOperationException($"Project code '{projectCode.Code}' already exists");
            }
            existing.Code = projectCode.Code;
        }
        existing.ProjectName = projectCode.ProjectName;
        existing.ClientName = projectCode.ClientName;
        existing.IsBillable = projectCode.IsBillable;
        existing.Status = projectCode.Status;
        existing.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ProjectCodes.UpdateAsync(existing);
        await _unitOfWork.CompleteAsync();

        return existing;
    }

    public async Task<bool> DeleteProjectCodeAsync(int projectCodeId)
    {
        var project = await _unitOfWork.ProjectCodes.GetByIdAsync(projectCodeId);
        if (project == null)
            return false;
        await _unitOfWork.ProjectCodes.DeleteAsync(project);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<IEnumerable<ProjectCode>> GetAllProjectCodesAsync()
    {
        return await _unitOfWork.ProjectCodes.GetAllAsync();
    }

    public async Task<IEnumerable<ProjectCode>> GetActiveProjectCodesAsync()
    {
        return await _unitOfWork.ProjectCodes.GetActiveProjectCodesAsync();
    }

    public async Task<ProjectCode?> GetProjectCodeByIdAsync(int id)
    {
        return await _unitOfWork.ProjectCodes.GetByIdAsync(id);
    }
}
