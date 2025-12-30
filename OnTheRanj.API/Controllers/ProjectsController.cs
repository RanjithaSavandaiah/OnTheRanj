using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.API.Controllers;

/// <summary>
/// Projects controller for managing project codes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Get all project codes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectCode>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllProjectCodesAsync();
        return Ok(projects);
    }

    /// <summary>
    /// Get active project codes
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ProjectCode>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var projects = await _projectService.GetActiveProjectCodesAsync();
        return Ok(projects);
    }

    /// <summary>
    /// Get project code by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectCode), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetProjectCodeByIdAsync(id);
        if (project == null)
        {
            return NotFound(new { message = "Project not found" });
        }
        return Ok(project);
    }

    /// <summary>
    /// Create new project code (Manager only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(ProjectCode), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ProjectCode projectCode)
    {
        try
        {
            // Use the correct claim for user ID (nameidentifier)
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Invalid token: nameidentifier claim missing" });
            }

            var managerId = int.Parse(userIdClaim.Value);
            var created = await _projectService.CreateProjectCodeAsync(projectCode, managerId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update project code (Manager only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(ProjectCode), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] DTOs.UpdateProjectRequest request)
    {
        try
        {
            var projectCode = new OnTheRanj.Core.Entities.ProjectCode
            {
                Id = id,
                Code = request.Code,
                ProjectName = request.ProjectName,
                ClientName = request.ClientName,
                IsBillable = request.IsBillable,
                Status = request.Status
            };
            var updated = await _projectService.UpdateProjectCodeAsync(projectCode);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Permanently delete a project code (Manager only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteProjectCodeAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Project not found" });
        }
        return Ok(new { message = "Project deleted successfully" });
    }
}
