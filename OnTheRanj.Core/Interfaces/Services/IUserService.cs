using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
}
