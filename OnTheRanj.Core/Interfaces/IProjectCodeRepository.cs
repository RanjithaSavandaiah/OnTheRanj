using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces;

/// <summary>
/// Specific repository interface for ProjectCode entity operations
/// </summary>
public interface IProjectCodeRepository : IRepository<ProjectCode>
{
    Task<IEnumerable<ProjectCode>> GetActiveProjectCodesAsync();
    Task<ProjectCode?> GetByCodeAsync(string code);
    Task<IEnumerable<ProjectCode>> GetByManagerIdAsync(int managerId);
}
