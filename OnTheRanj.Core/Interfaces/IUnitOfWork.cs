namespace OnTheRanj.Core.Interfaces;

/// <summary>
/// Unit of Work pattern interface
/// Manages transactions and coordinates work of multiple repositories
/// Ensures atomic operations across multiple entities
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Assignment repository instance
    /// </summary>
    /// <summary>
    /// User repository instance
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// ProjectCode repository instance
    /// </summary>
    IProjectCodeRepository ProjectCodes { get; }

    /// <summary>
    /// ProjectAssignment repository instance
    /// </summary>
    IProjectAssignmentRepository ProjectAssignments { get; }

    /// <summary>
    /// Timesheet repository instance
    /// </summary>
    ITimesheetRepository Timesheets { get; }

    Task<int> CompleteAsync();

    /// Begins a database transaction
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync();
}
