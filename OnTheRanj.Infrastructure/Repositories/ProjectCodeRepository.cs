using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.Infrastructure.Repositories;

/// <summary>
/// ProjectCode repository implementation
/// </summary>
public class ProjectCodeRepository : Repository<ProjectCode>, IProjectCodeRepository
{
    public ProjectCodeRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// Gets active project codes
    /// </summary>
    public async Task<IEnumerable<ProjectCode>> GetActiveProjectCodesAsync()
    {
        return await _dbSet
            .Where(pc => pc.Status == ProjectStatus.Active)
            .OrderBy(pc => pc.Code)
            .ToListAsync();
    }

    /// <summary>
    /// Gets project code by code string
    /// </summary>
    public async Task<ProjectCode?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(pc => pc.Code == code);
    }

    /// <summary>
    /// Gets project codes created by a manager
    /// </summary>
    public async Task<IEnumerable<ProjectCode>> GetByManagerIdAsync(int managerId)
    {
        return await _dbSet
            .Where(pc => pc.CreatedBy == managerId)
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync();
    }
}
