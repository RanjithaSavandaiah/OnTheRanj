namespace OnTheRanj.Core.DTOs;

public class DashboardSummaryDto
{
    public List<EmployeeHoursSummary> Employees { get; set; } = new();
    public List<ProjectHoursSummary> Projects { get; set; } = new();
    public BillableHoursSummary BillableVsNonBillable { get; set; } = new();
}
