using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnTheRanj.Core.Interfaces;
using OnTheRanj.Infrastructure.Data;

namespace OnTheRanj.Infrastructure.Repositories;

/// <summary>
/// Base repository implementation with generic CRUD operations
/// Implements Repository Pattern for data access abstraction
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    public virtual IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Gets entity by ID
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Gets all entities
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Finds entities matching a predicate
    /// </summary>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Finds first entity matching predicate or returns null
    /// </summary>
    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Adds a new entity
    /// </summary>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes an entity
    /// </summary>
    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks if any entity matches the predicate
    /// </summary>
    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    /// <summary>
    /// Gets count of entities matching predicate
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }
}
