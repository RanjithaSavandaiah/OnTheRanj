using OnTheRanj.Core.Entities;

namespace OnTheRanj.Core.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task<User> RegisterAsync(string fullName, string email, string password, string role);
    bool ValidateToken(string token);
    int GetUserIdFromToken(string token);
}
