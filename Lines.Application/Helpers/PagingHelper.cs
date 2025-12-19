using Lines.Application.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Helpers;

public static class PagingHelper
{
    public static async Task<PagingDto<T>> CreateAsync<T>(
        IQueryable<T> source, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken); 
        
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagingDto<T>(items, count, pageNumber, pageSize);
    }
}
