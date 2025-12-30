namespace OnTheRanj.API.DTOs;

public class TimesheetDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ProjectCodeId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal HoursWorked { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ManagerComments { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
