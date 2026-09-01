using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shaghaf.Application.Common.Interfaces;

namespace Shaghaf.Infrastructure.Persistence.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    private readonly ShaghafDbContext _context;
    private readonly DbSet<TEntity> _set;

    public Repository(ShaghafDbContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query(bool asTracking = false) =>
        asTracking ? _set : _set.AsNoTracking();

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        await _set.FindAsync([id], cancellationToken);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
        _set.AsNoTracking().AnyAsync(predicate, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _set.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity) => _set.Update(entity);

    public void Remove(TEntity entity) => _set.Remove(entity);
}
