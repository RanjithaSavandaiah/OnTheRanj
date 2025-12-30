using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services;

/// <summary>
/// Authentication service implementation with JWT token generation
/// Handles user login, registration, and token validation
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly string _jwtSecret;
    private readonly int _jwtExpiryHours;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        
        // Get JWT settings from configuration
        _jwtSecret = _configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyForJWTTokenGenerationWithMinimum256Bits";
        _jwtExpiryHours = int.TryParse(_configuration["Jwt:ExpiryHours"], out var hours) ? hours : 24;
    }

    /// <summary>
    /// Authenticates user and generates JWT token
    /// </summary>
    public async Task<string> LoginAsync(string email, string password)
    {
        // Find user by email
        var user = await _unitOfWork.Users.GetByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid email or password");
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!passwordMatch)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        // Generate JWT token
        return GenerateJwtToken(user);
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    public async Task<User> RegisterAsync(string fullName, string email, string password, string role)
    {
        // Check if user already exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        // Hash password using BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // Create new user
        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    /// <summary>
    /// Validates JWT token
    /// </summary>
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts user ID from JWT token
    /// </summary>
    public int GetUserIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            throw new InvalidOperationException("Invalid token");
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Invalid token");
        }
    }

    /// <summary>
    /// Generates JWT token for authenticated user
    /// </summary>
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(_jwtExpiryHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
