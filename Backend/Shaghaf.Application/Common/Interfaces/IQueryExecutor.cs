using Shaghaf.Application.Common.Models;

namespace Shaghaf.Application.Common.Interfaces;

/// <summary>
/// Executes LINQ queries asynchronously so the application layer stays free of
/// a direct EF Core dependency.
/// </summary>
public interface IQueryExecutor
{
    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<PagedResult<T>> ToPagedResultAsync<T>(IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default);
}
