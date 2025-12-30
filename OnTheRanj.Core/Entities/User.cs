namespace OnTheRanj.Core.Entities;

/// <summary>
/// Represents a user in the system (Employee or Manager)
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the user
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address (used for login)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// User role: "Employee" or "Manager"
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the user account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for project assignments
    /// </summary>
    public virtual ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();

    /// <summary>
    /// Navigation property for timesheets
    /// </summary>
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
