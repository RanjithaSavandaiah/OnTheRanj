namespace OnTheRanj.Core.Entities;

/// <summary>
/// Represents a timesheet entry submitted by an employee
/// </summary>
public class Timesheet
{
    /// <summary>
    /// Unique identifier for the timesheet entry
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the employee who submitted the timesheet
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// ID of the project code for which time is logged
    /// </summary>
    public int ProjectCodeId { get; set; }

    /// <summary>
    /// Date for which time is logged
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Number of hours worked (max 24 per day)
    /// </summary>
    public decimal HoursWorked { get; set; }

    /// <summary>
    /// Description of work performed
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Timesheet status: "Draft", "Submitted", "Approved", "Rejected"
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// Comments from manager (required for rejection)
    /// </summary>
    public string? ManagerComments { get; set; }

    /// <summary>
    /// ID of the manager who approved/rejected the timesheet
    /// </summary>
    public int? ReviewedBy { get; set; }

    /// <summary>
    /// Date when the timesheet was reviewed
    /// </summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Date when the timesheet was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the timesheet was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Navigation property to the employee
    /// </summary>
    public virtual User Employee { get; set; } = null!;

    /// <summary>
    /// Navigation property to the project code
    /// </summary>
    public virtual ProjectCode ProjectCode { get; set; } = null!;

    /// <summary>
    /// Navigation property to the reviewing manager
    /// </summary>
    public virtual User? Reviewer { get; set; }
}
