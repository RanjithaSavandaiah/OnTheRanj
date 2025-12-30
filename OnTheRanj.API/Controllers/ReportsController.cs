using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.API.Controllers;

/// <summary>
/// Reports controller for timesheet and project analytics
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Get employee-wise hours summary
    /// </summary>
    [HttpGet("employee-hours")]
    public async Task<IActionResult> GetEmployeeHoursSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _reportService.GetEmployeeHoursSummaryAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Get project-wise hours summary
    /// </summary>
    [HttpGet("project-hours")]
    public async Task<IActionResult> GetProjectHoursSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _reportService.GetProjectHoursSummaryAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Get billable vs non-billable hours summary
    /// </summary>
    [HttpGet("billable-hours")]
    public async Task<IActionResult> GetBillableVsNonBillableHours([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _reportService.GetBillableVsNonBillableHoursAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    /// <summary>
    /// Get dashboard summary
    /// </summary>
    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> GetDashboardSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _reportService.GetDashboardSummaryAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}