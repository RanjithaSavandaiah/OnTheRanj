using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.Infrastructure.Repositories;

/// <summary>
/// User repository implementation with specific user operations
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// Gets user by email address
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <summary>
    /// Gets all employees
    /// </summary>
    public async Task<IEnumerable<User>> GetEmployeesAsync()
    {
        return await _dbSet
            .Where(u => u.Role == UserRole.Employee && u.IsActive)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all managers
    /// </summary>
    public async Task<IEnumerable<User>> GetManagersAsync()
    {
        return await _dbSet
            .Where(u => u.Role == UserRole.Manager && u.IsActive)
            .ToListAsync();
    }
}
