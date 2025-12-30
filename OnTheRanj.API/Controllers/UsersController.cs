using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
}
