namespace OnTheRanj.API.DTOs;

public class CreateAssignmentDto
{
    public int EmployeeId { get; set; }
    public int ProjectCodeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
