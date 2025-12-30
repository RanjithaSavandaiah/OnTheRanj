namespace OnTheRanj.Core.DTOs;

public class EmployeeHoursSummary
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public decimal TotalHours { get; set; }
    public decimal BillableHours { get; set; }
    public decimal NonBillableHours { get; set; }
}
