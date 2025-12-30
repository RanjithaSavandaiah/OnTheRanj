namespace OnTheRanj.API.DTOs;

public class TimesheetWeekEntryDto
{
    public int EmployeeId { get; set; }
    public int ProjectCodeId { get; set; }
    public DateTime Date { get; set; }
    public decimal HoursWorked { get; set; }
    public string Description { get; set; } = string.Empty;
}
