using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectCode> CreateProjectCodeAsync(ProjectCode projectCode, int managerId);
    Task<ProjectCode> UpdateProjectCodeAsync(ProjectCode projectCode);
    Task<bool> DeleteProjectCodeAsync(int projectCodeId);
    Task<IEnumerable<ProjectCode>> GetAllProjectCodesAsync();
    Task<IEnumerable<ProjectCode>> GetActiveProjectCodesAsync();
    Task<ProjectCode?> GetProjectCodeByIdAsync(int id);
}
