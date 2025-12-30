namespace OnTheRanj.Core.Entities;

/// <summary>
/// Represents the assignment of a project code to an employee for a specific date range
/// </summary>
public class ProjectAssignment
{
    /// <summary>
    /// Unique identifier for the project assignment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the employee assigned to the project
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// ID of the project code being assigned
    /// </summary>
    public int ProjectCodeId { get; set; }

    /// <summary>
    /// Start date of the assignment
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the assignment (null means ongoing)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Date when the assignment was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID of the manager who created the assignment
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Navigation property to the employee
    /// </summary>
    public virtual User Employee { get; set; } = null!;

    /// <summary>
    /// Navigation property to the project code
    /// </summary>
    public virtual ProjectCode ProjectCode { get; set; } = null!;
}
