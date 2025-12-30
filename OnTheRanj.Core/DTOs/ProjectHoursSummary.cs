namespace OnTheRanj.Core.DTOs;

public class ProjectHoursSummary
{
    public int ProjectCodeId { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public decimal TotalHours { get; set; }
    public int EmployeeCount { get; set; }
}
