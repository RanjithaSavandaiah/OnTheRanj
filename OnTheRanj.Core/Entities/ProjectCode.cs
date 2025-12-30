namespace OnTheRanj.Core.Entities;

/// <summary>
/// Represents a project code that can be assigned to employees for timesheet submission
/// </summary>
public class ProjectCode
{
    /// <summary>
    /// Unique identifier for the project code
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique project code (e.g., "PROJ001")
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the project
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;

    /// <summary>
    /// Client name for the project
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the project is billable to client
    /// </summary>
    public bool IsBillable { get; set; }

    /// <summary>
    /// Project status: "Active" or "Inactive"
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Date when the project was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the project was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// ID of the manager who created the project
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Navigation property for project assignments
    /// </summary>
    public virtual ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();

    /// <summary>
    /// Navigation property for timesheets
    /// </summary>
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
