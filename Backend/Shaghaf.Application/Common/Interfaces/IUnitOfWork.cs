namespace Shaghaf.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IRepository<TEntity, Guid> Repository<TEntity>() where TEntity : class;

    IRepository<TEntity, int> RepositoryInt<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default);
}
