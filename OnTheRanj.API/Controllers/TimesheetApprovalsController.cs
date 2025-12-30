using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.API.DTOs;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class TimesheetApprovalsController : ControllerBase
{
    private readonly ITimesheetApprovalService _approvalService;
    public TimesheetApprovalsController(ITimesheetApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

    // GET: api/timesheetapprovals/pending
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<TimesheetApprovalDto>>> GetPendingTimesheets()
    {
        var timesheets = await _approvalService.GetPendingTimesheetsAsync();
        var dtos = timesheets.Select(ts => new TimesheetApprovalDto
        {
            Id = ts.Id,
            EmployeeName = ts.Employee?.FullName ?? $"Employee #{ts.EmployeeId}",
            ProjectName = ts.ProjectCode?.ProjectName ?? $"Project #{ts.ProjectCodeId}",
            Date = ts.Date,
            HoursWorked = ts.HoursWorked,
            Status = ts.Status
        }).ToList();
        return Ok(dtos);
    }

    // GET: api/timesheetapprovals/status/{status}
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TimesheetApprovalDto>>> GetTimesheetsByStatus(string status)
    {
        var timesheets = await _approvalService.GetTimesheetsByStatusAsync(status);
        var dtos = timesheets.Select(ts => new TimesheetApprovalDto
        {
            Id = ts.Id,
            EmployeeName = ts.Employee?.FullName ?? $"Employee #{ts.EmployeeId}",
            ProjectName = ts.ProjectCode?.ProjectName ?? $"Project #{ts.ProjectCodeId}",
            Date = ts.Date,
            HoursWorked = ts.HoursWorked,
            Status = ts.Status
        }).ToList();
        return Ok(dtos);
    }

    // POST: api/timesheetapprovals/approve/{id}
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveTimesheet(int id)
    {
        // TODO: Get managerId from claims or context
        int managerId = 1; // Placeholder
        var result = await _approvalService.ApproveTimesheetAsync(id, managerId);
        if (!result)
            return BadRequest("Approval failed.");
        return Ok();
    }

    // POST: api/timesheetapprovals/reject/{id}
    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectTimesheet(int id, [FromBody] RejectTimesheetRequest request)
    {
        // TODO: Get managerId from claims or context
        int managerId = 1; // Placeholder
        if (string.IsNullOrWhiteSpace(request?.Comments))
            return BadRequest("Rejection comments are required.");
        var result = await _approvalService.RejectTimesheetAsync(id, managerId, request.Comments);
        if (!result)
            return BadRequest("Rejection failed.");
        return Ok();
    }
}
