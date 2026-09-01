using System.Linq.Expressions;

namespace Shaghaf.Application.Common.Interfaces;

/// <summary>
/// Generic read/write access to an aggregate. Queries are tracked only when
/// explicitly requested so read paths stay cheap.
/// </summary>
public interface IRepository<TEntity, TKey> where TEntity : class
{
    IQueryable<TEntity> Query(bool asTracking = false);

    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}
