using Microsoft.EntityFrameworkCore.Storage;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Data;
using OnTheRanj.Infrastructure.Repositories;

namespace OnTheRanj.Infrastructure.UnitOfWork;

/// <summary>
/// Unit of Work implementation
/// Coordinates the work of multiple repositories and maintains a single database context
/// Implements transaction management for atomic operations
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    // Repository instances
    private IUserRepository? _users;
    private IProjectCodeRepository? _projectCodes;
    private IProjectAssignmentRepository? _projectAssignments;
    private ITimesheetRepository? _timesheets;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets User repository instance (lazy initialization)
    /// </summary>
    public IUserRepository Users
    {
        get
        {
            _users ??= new UserRepository(_context);
            return _users;
        }
    }

    /// <summary>
    /// Gets ProjectCode repository instance (lazy initialization)
    /// </summary>
    public IProjectCodeRepository ProjectCodes
    {
        get
        {
            _projectCodes ??= new ProjectCodeRepository(_context);
            return _projectCodes;
        }
    }

    /// <summary>
    /// Gets ProjectAssignment repository instance (lazy initialization)
    /// </summary>
    public IProjectAssignmentRepository ProjectAssignments
    {
        get
        {
            _projectAssignments ??= new ProjectAssignmentRepository(_context);
            return _projectAssignments;
        }
    }

    /// <summary>
    /// Gets Timesheet repository instance (lazy initialization)
    /// </summary>
    public ITimesheetRepository Timesheets
    {
        get
        {
            _timesheets ??= new TimesheetRepository(_context);
            return _timesheets;
        }
    }

    /// <summary>
    /// Saves all changes made in this unit of work to the database
    /// </summary>
    /// <returns>Number of state entries written to the database</returns>
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Disposes the context and transaction
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
