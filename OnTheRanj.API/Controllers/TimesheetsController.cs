using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.Core.Entities;
using OnTheRanj.Core.Interfaces.Services;


namespace OnTheRanj.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimesheetsController : ControllerBase
{
    private readonly ITimesheetService _timesheetService;
    public TimesheetsController(ITimesheetService timesheetService)
    {
        _timesheetService = timesheetService;
    }

    // POST: api/timesheets/week
    [HttpPost("week")]
    public async Task<IActionResult> SubmitWeek([FromBody] DTOs.TimesheetWeekSubmitRequest request)
    {
        if (request == null || request.Entries == null || request.Entries.Count == 0)
            return BadRequest(new { message = "No timesheet entries provided." });

        var results = new List<object>();
        foreach (var entry in request.Entries)
        {
            try
            {
                var timesheet = new Timesheet
                {
                    EmployeeId = request.EmployeeId,
                    ProjectCodeId = entry.ProjectCodeId,
                    Date = entry.Date,
                    HoursWorked = entry.HoursWorked,
                    Description = entry.Description ?? string.Empty
                };
                var created = await _timesheetService.CreateTimesheetAsync(timesheet);
                results.Add(new { success = true, date = entry.Date, projectCodeId = entry.ProjectCodeId });
            }
            catch (InvalidOperationException ex)
            {
                results.Add(new { success = false, date = entry.Date, projectCodeId = entry.ProjectCodeId, message = ex.Message });
            }
            catch (Exception ex)
            {
                results.Add(new { success = false, date = entry.Date, projectCodeId = entry.ProjectCodeId, message = "Internal server error.", detail = ex.Message });
            }
        }
        return Ok(new { results });
    }

    // PATCH: api/timesheets/{id}/submit
    [HttpPatch("{id}/submit")]
    public async Task<ActionResult<DTOs.TimesheetDto>> SubmitTimesheet(int id)
    {
        try
        {
            var success = await _timesheetService.SubmitTimesheetAsync(id);
            if (!success)
            {
                return NotFound();
            }
            var updated = await _timesheetService.GetTimesheetByIdAsync(id);
            if (updated == null)
            {
                return NotFound();
            }
            var result = new DTOs.TimesheetDto
            {
                Id = updated.Id,
                EmployeeId = updated.EmployeeId,
                ProjectCodeId = updated.ProjectCodeId,
                ProjectName = updated.ProjectCode?.ProjectName ?? string.Empty,
                Date = updated.Date,
                HoursWorked = updated.HoursWorked,
                Description = updated.Description,
                Status = updated.Status.ToString(),
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error.", detail = ex.Message });
        }
    }

    // GET: api/timesheets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DTOs.TimesheetDto>>> GetAllTimesheets()
    {
        var timesheets = await _timesheetService.GetAllTimesheetsAsync();
        var dtos = timesheets.Select(ts => new DTOs.TimesheetDto
        {
            Id = ts.Id,
            EmployeeId = ts.EmployeeId,
            ProjectCodeId = ts.ProjectCodeId,
            ProjectName = ts.ProjectCode?.ProjectName ?? string.Empty,
            Date = ts.Date,
            HoursWorked = ts.HoursWorked,
            Description = ts.Description,
            Status = ts.Status.ToString(),
            ManagerComments = ts.ManagerComments,
            CreatedAt = ts.CreatedAt,
            UpdatedAt = ts.UpdatedAt
        });
        return Ok(dtos);
    }

    // GET: api/timesheets/pending
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<DTOs.TimesheetDto>>> GetPendingTimesheets()
    {
        var timesheets = await _timesheetService.GetAllTimesheetsAsync();
        var pending = timesheets.Where(ts => ts.Status.ToString() == "Submitted");
        var dtos = pending.Select(ts => new DTOs.TimesheetDto
        {
            Id = ts.Id,
            EmployeeId = ts.EmployeeId,
            ProjectCodeId = ts.ProjectCodeId,
            ProjectName = ts.ProjectCode?.ProjectName ?? string.Empty,
            Date = ts.Date,
            HoursWorked = ts.HoursWorked,
            Description = ts.Description,
            Status = ts.Status.ToString(),
            ManagerComments = ts.ManagerComments,
            CreatedAt = ts.CreatedAt,
            UpdatedAt = ts.UpdatedAt
        });
        return Ok(dtos);
    }

    // GET: api/timesheets/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]

    public async Task<ActionResult<IEnumerable<DTOs.TimesheetDto>>> GetEmployeeTimesheets(int employeeId)
    {
        var timesheets = await _timesheetService.GetEmployeeTimesheetsAsync(employeeId);
        foreach (var ts in timesheets)
        {
            Console.WriteLine($"TimesheetId: {ts.Id}, ProjectCodeId: {ts.ProjectCodeId}, ProjectCode: {(ts.ProjectCode != null ? ts.ProjectCode.ProjectName : "NULL")}");
        }
        var dtos = timesheets.Select(ts => new DTOs.TimesheetDto
        {
            Id = ts.Id,
            EmployeeId = ts.EmployeeId,
            ProjectCodeId = ts.ProjectCodeId,
            ProjectName = ts.ProjectCode?.ProjectName ?? string.Empty,
            Date = ts.Date,
            HoursWorked = ts.HoursWorked,
            Description = ts.Description,
            Status = ts.Status.ToString(),
            ManagerComments = ts.ManagerComments,
            CreatedAt = ts.CreatedAt,
            UpdatedAt = ts.UpdatedAt
        });
        return Ok(dtos);
    }

    // POST: api/timesheets
    [HttpPost]
    public async Task<ActionResult<DTOs.TimesheetDto>> CreateTimesheet([FromBody] DTOs.TimesheetDto dto)
    {
        try
        {
            var timesheet = new Timesheet
            {
                EmployeeId = dto.EmployeeId,
                ProjectCodeId = dto.ProjectCodeId,
                Date = dto.Date,
                HoursWorked = dto.HoursWorked,
                Description = dto.Description ?? string.Empty
            };
            var created = await _timesheetService.CreateTimesheetAsync(timesheet);
            var result = new DTOs.TimesheetDto
            {
                Id = created.Id,
                EmployeeId = created.EmployeeId,
                ProjectCodeId = created.ProjectCodeId,
                ProjectName = created.ProjectCode?.ProjectName ?? string.Empty,
                Date = created.Date,
                HoursWorked = created.HoursWorked,
                Description = created.Description,
                Status = created.Status.ToString(),
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
            return CreatedAtAction(nameof(GetEmployeeTimesheets), new { employeeId = result.EmployeeId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error.", detail = ex.Message });
        }
    }

    // PUT: api/timesheets/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<DTOs.TimesheetDto>> UpdateTimesheet(int id, [FromBody] DTOs.TimesheetDto dto)
    {
        try
        {
            var existing = await _timesheetService.GetTimesheetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            // Update fields
            existing.ProjectCodeId = dto.ProjectCodeId;
            existing.Date = dto.Date;
            existing.HoursWorked = dto.HoursWorked;
            existing.Description = dto.Description ?? string.Empty;
            // Optionally update status, etc. if needed
            var updated = await _timesheetService.UpdateTimesheetAsync(existing);
            var result = new DTOs.TimesheetDto
            {
                Id = updated.Id,
                EmployeeId = updated.EmployeeId,
                ProjectCodeId = updated.ProjectCodeId,
                ProjectName = updated.ProjectCode?.ProjectName ?? string.Empty,
                Date = updated.Date,
                HoursWorked = updated.HoursWorked,
                Description = updated.Description,
                Status = updated.Status.ToString(),
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error.", detail = ex.Message });
        }
    }
}
