namespace OnTheRanj.API.DTOs;

public class TimesheetApprovalDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal HoursWorked { get; set; }
    public string Status { get; set; } = string.Empty;
}
