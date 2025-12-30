namespace OnTheRanj.API.DTOs;

public class TimesheetWeekSubmitRequest
{
    public int EmployeeId { get; set; }
    public List<TimesheetWeekEntryDto> Entries { get; set; } = new List<TimesheetWeekEntryDto>();
}
