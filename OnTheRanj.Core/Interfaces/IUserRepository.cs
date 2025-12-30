using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces;

/// <summary>
/// Specific repository interface for User entity operations
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetEmployeesAsync();
    Task<IEnumerable<User>> GetManagersAsync();
}
