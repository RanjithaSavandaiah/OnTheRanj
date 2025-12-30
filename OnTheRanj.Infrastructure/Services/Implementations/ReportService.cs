using OnTheRanj.Core.DTOs;
using OnTheRanj.Core.Enums;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Core.Interfaces.Services;

namespace OnTheRanj.Infrastructure.Services.Implementations;

/// <summary>
/// Report generation service with EF Core aggregations
/// All aggregations done using LINQ queries
/// </summary>
public class ReportService : IReportService
{
    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var timesheets = await _unitOfWork.Timesheets.GetByDateRangeAsync(startDate, endDate);
        var approved = timesheets.Where(t => t.Status == TimesheetStatus.Approved).ToList();

        var employees = approved
            .GroupBy(t => new { t.EmployeeId, t.Employee.FullName })
            .Select(g => new EmployeeHoursSummary
            {
                EmployeeId = g.Key.EmployeeId,
                EmployeeName = g.Key.FullName,
                TotalHours = (decimal)g.Sum(t => (double)t.HoursWorked),
                BillableHours = (decimal)g.Where(t => t.ProjectCode.IsBillable).Sum(t => (double)t.HoursWorked),
                NonBillableHours = (decimal)g.Where(t => !t.ProjectCode.IsBillable).Sum(t => (double)t.HoursWorked)
            })
            .OrderBy(e => e.EmployeeName)
            .ToList();

        var projects = approved
            .GroupBy(t => new {
                t.ProjectCodeId,
                t.ProjectCode.Code,
                t.ProjectCode.ProjectName,
                t.ProjectCode.ClientName
            })
            .Select(g => new ProjectHoursSummary
            {
                ProjectCodeId = g.Key.ProjectCodeId,
                ProjectCode = g.Key.Code,
                ProjectName = g.Key.ProjectName,
                ClientName = g.Key.ClientName,
                TotalHours = (decimal)g.Sum(t => (double)t.HoursWorked),
                EmployeeCount = g.Select(t => t.EmployeeId).Distinct().Count()
            })
            .OrderBy(p => p.ProjectCode)
            .ToList();

        var billable = (decimal)approved.Where(t => t.ProjectCode.IsBillable).Sum(t => (double)t.HoursWorked);
        var nonBillable = (decimal)approved.Where(t => !t.ProjectCode.IsBillable).Sum(t => (double)t.HoursWorked);
        var totalHours = billable + nonBillable;
        var billableVsNonBillable = new BillableHoursSummary
        {
            TotalBillableHours = billable,
            TotalNonBillableHours = nonBillable,
            TotalHours = totalHours,
            BillablePercentage = totalHours > 0 ? (decimal)((double)billable / (double)totalHours * 100) : 0
        };

        return new DashboardSummaryDto
        {
            Employees = employees,
            Projects = projects,
            BillableVsNonBillable = billableVsNonBillable
        };
    }

    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IEnumerable<EmployeeHoursSummary>> GetEmployeeHoursSummaryAsync(
        DateTime startDate, DateTime endDate)
    {
        var timesheets = await _unitOfWork.Timesheets.GetByDateRangeAsync(startDate, endDate);
        
        var summary = timesheets
            .Where(t => t.Status == TimesheetStatus.Approved)
            .GroupBy(t => new { t.EmployeeId, t.Employee.FullName })
            .Select(g => new EmployeeHoursSummary
            {
                EmployeeId = g.Key.EmployeeId,
                EmployeeName = g.Key.FullName,
                TotalHours = (decimal)g.Sum(t => (double)t.HoursWorked),
                BillableHours = (decimal)g.Where(t => t.ProjectCode.IsBillable).Sum(t => (double)t.HoursWorked),
                NonBillableHours = (decimal)g.Where(t => !t.ProjectCode.IsBillable).Sum(t => (double)t.HoursWorked)
            })
            .OrderBy(s => s.EmployeeName)
            .ToList();

        return summary;
    }

    public async Task<IEnumerable<ProjectHoursSummary>> GetProjectHoursSummaryAsync(
        DateTime startDate, DateTime endDate)
    {
        var timesheets = await _unitOfWork.Timesheets.GetByDateRangeAsync(startDate, endDate);
        
        var summary = timesheets
            .Where(t => t.Status == TimesheetStatus.Approved)
            .GroupBy(t => new
            {
                t.ProjectCodeId,
                t.ProjectCode.Code,
                t.ProjectCode.ProjectName,
                t.ProjectCode.ClientName
            })
            .Select(g => new ProjectHoursSummary
            {
                ProjectCodeId = g.Key.ProjectCodeId,
                ProjectCode = g.Key.Code,
                ProjectName = g.Key.ProjectName,
                ClientName = g.Key.ClientName,
                TotalHours = (decimal)g.Sum(t => (double)t.HoursWorked),
                EmployeeCount = g.Select(t => t.EmployeeId).Distinct().Count()
            })
            .OrderBy(s => s.ProjectCode)
            .ToList();

        return summary;
    }

    public async Task<BillableHoursSummary> GetBillableVsNonBillableHoursAsync(
        DateTime startDate, DateTime endDate)
    {
        var timesheets = await _unitOfWork.Timesheets.GetByDateRangeAsync(startDate, endDate);
        
        var approvedTimesheets = timesheets.Where(t => t.Status == TimesheetStatus.Approved).ToList();
        
        var billableHours = approvedTimesheets
            .Where(t => t.ProjectCode.IsBillable)
            .Sum(t => (double)t.HoursWorked);
        
        var nonBillableHours = approvedTimesheets
            .Where(t => !t.ProjectCode.IsBillable)
            .Sum(t => (double)t.HoursWorked);
        
        var totalHours = billableHours + nonBillableHours;
        
        return new BillableHoursSummary
        {
            TotalBillableHours = (decimal)billableHours,
            TotalNonBillableHours = (decimal)nonBillableHours,
            TotalHours = (decimal)totalHours,
            BillablePercentage = totalHours > 0 ? (decimal)((billableHours / totalHours) * 100) : 0
        };
    }
}
