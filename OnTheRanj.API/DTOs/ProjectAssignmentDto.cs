namespace OnTheRanj.API.DTOs;

public class ProjectAssignmentDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ProjectCodeId { get; set; }
    public string? ProjectName { get; set; } // For display
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public string? EmployeeName { get; set; } // For display
}
