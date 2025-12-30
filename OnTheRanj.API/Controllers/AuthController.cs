using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.API.Controllers;

/// <summary>
/// Authentication controller for user login and registration
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// User login endpoint
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token on successful authentication</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new LoginResponse { Token = token, Message = "Login successful" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// User registration endpoint
    /// </summary>
    /// <param name="request">Registration details</param>
    /// <returns>Created user information</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(
                request.FullName,
                request.Email,
                request.Password,
                request.Role);

            return CreatedAtAction(nameof(Register), new RegisterResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Message = "User registered successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
