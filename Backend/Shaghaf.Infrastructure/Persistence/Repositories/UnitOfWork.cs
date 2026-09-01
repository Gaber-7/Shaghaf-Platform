using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Shaghaf.Application.Common.Interfaces;

namespace Shaghaf.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShaghafDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(ShaghafDbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity, Guid> Repository<TEntity>() where TEntity : class =>
        (IRepository<TEntity, Guid>)_repositories.GetOrAdd(
            typeof(Repository<TEntity, Guid>),
            _ => new Repository<TEntity, Guid>(_context));

    public IRepository<TEntity, int> RepositoryInt<TEntity>() where TEntity : class =>
        (IRepository<TEntity, int>)_repositories.GetOrAdd(
            typeof(Repository<TEntity, int>),
            _ => new Repository<TEntity, int>(_context));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        Func<CancellationToken, Task> transactional = async token =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(token);
            await operation(token);
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        };

        await strategy.ExecuteAsync(transactional, cancellationToken);
    }
}
