using Microsoft.EntityFrameworkCore;
using Shaghaf.Application.Common.Interfaces;
using Shaghaf.Application.Common.Models;

namespace Shaghaf.Infrastructure.Persistence;

public class QueryExecutor : IQueryExecutor
{
    public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
        query.ToListAsync(cancellationToken);

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
        query.FirstOrDefaultAsync(cancellationToken);

    public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
        query.CountAsync(cancellationToken);

    public async Task<PagedResult<T>> ToPagedResultAsync<T>(IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, page, pageSize);
    }
}
