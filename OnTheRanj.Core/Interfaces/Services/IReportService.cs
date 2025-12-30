namespace OnTheRanj.Core.Interfaces.Services;

public interface IReportService
{
    Task<IEnumerable<DTOs.EmployeeHoursSummary>> GetEmployeeHoursSummaryAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<DTOs.ProjectHoursSummary>> GetProjectHoursSummaryAsync(DateTime startDate, DateTime endDate);
    Task<DTOs.BillableHoursSummary> GetBillableVsNonBillableHoursAsync(DateTime startDate, DateTime endDate);

    Task<DTOs.DashboardSummaryDto> GetDashboardSummaryAsync(DateTime startDate, DateTime endDate);
}
