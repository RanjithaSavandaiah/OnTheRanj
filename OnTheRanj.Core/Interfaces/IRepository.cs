using System.Linq.Expressions;

namespace OnTheRanj.Core.Interfaces;

/// <summary>
/// Generic repository interface for data access operations
/// Implements Repository Pattern for abstraction of data layer
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets entity by ID
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all entities
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Finds entities matching a condition
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Finds first entity matching a condition or null
    /// </summary>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Adds a new entity
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Checks if any entity matches a condition
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets count of entities matching a condition
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
}
