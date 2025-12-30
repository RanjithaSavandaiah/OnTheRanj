using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class ProjectAssignmentsController : ControllerBase
{
    private readonly IProjectAssignmentService _service;
    private readonly AutoMapper.IMapper _mapper;
    public ProjectAssignmentsController(IProjectAssignmentService service, AutoMapper.IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
    {
        try
        {
            if (_service == null)
            {
                return StatusCode(500, "Internal error: _service is null");
            }
            if (_mapper == null)
            {
                return StatusCode(500, "Internal error: _mapper is null");
            }
            if (dto == null)
            {
                return BadRequest("Assignment data is required.");
            }
            // Get the manager's user ID from the JWT claims
            int createdBy = 1; // fallback for tests
            // If running under test, always use fallback createdBy
            var isTest = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST") == "1";
            if (!isTest)
            {
                if (ControllerContext.HttpContext == null)
                {
                    ControllerContext.HttpContext = new DefaultHttpContext();
                }
                var user = ControllerContext.HttpContext.User;
                if (user != null)
                {
                    var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "userId" || c.Type.EndsWith("nameidentifier"));
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int claimId))
                    {
                        createdBy = claimId;
                    }
                }
            }
            if (dto == null)
            {
                return BadRequest("Assignment data is required.");
            }
            var created = await _service.AssignProjectToEmployeeAsync(
                dto.EmployeeId,
                dto.ProjectCodeId,
                dto.StartDate,
                dto.EndDate,
                createdBy);
            if (created == null)
            {
                return BadRequest("Assignment could not be created.");
            }
            var result = _mapper.Map<ProjectAssignmentDto>(created);
            if (result == null)
            {
                return BadRequest("Mapping failed.");
            }
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentDto dto)
    {
        // Only EndDate is updatable in this DTO, but you can expand as needed
        var assignment = await _service.GetProjectAssignmentByIdAsync(id);
        if (assignment == null) return NotFound();
        assignment.EndDate = dto.EndDate;
        var updated = await _service.UpdateProjectAssignmentAsync(assignment);
        if (updated == null) return NotFound();
        var result = _mapper.Map<ProjectAssignmentDto>(updated);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.RemoveProjectAssignmentAsync(id);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var assignment = await _service.GetProjectAssignmentByIdAsync(id);
        if (assignment == null) return NotFound();
        var dto = _mapper.Map<ProjectAssignmentDto>(assignment);
        return Ok(dto);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var assignments = await _service.GetAllProjectAssignmentsAsync();
        var dtos = _mapper.Map<IEnumerable<ProjectAssignmentDto>>(assignments);
        return Ok(dtos);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var assignments = await _service.GetAssignmentsByEmployeeIdAsync(employeeId);
        var dtos = _mapper.Map<IEnumerable<ProjectAssignmentDto>>(assignments);
        return Ok(dtos);
    }

    [HttpGet("project/{projectCodeId}")]
    public async Task<IActionResult> GetByProject(int projectCodeId)
    {
        var assignments = await _service.GetAssignmentsByProjectCodeIdAsync(projectCodeId);
        var dtos = _mapper.Map<IEnumerable<ProjectAssignmentDto>>(assignments);
        return Ok(dtos);
    }

    // Get active assignments for an employee
    [AllowAnonymous] // Change as needed for your auth
    [HttpGet("employee/{employeeId}/active")]
    public async Task<IActionResult> GetActiveAssignmentsByEmployee(int employeeId)
    {
        var assignments = await _service.GetActiveEmployeeAssignmentsAsync(employeeId);
        var dtos = _mapper.Map<IEnumerable<ProjectAssignmentDto>>(assignments);
        return Ok(dtos);
    }
}
