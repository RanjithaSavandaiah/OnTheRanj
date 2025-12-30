namespace OnTheRanj.API.DTOs;

public class UpdateProjectRequest
{
    public string Code { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public bool IsBillable { get; set; }
    public string Status { get; set; } = "Active";
}