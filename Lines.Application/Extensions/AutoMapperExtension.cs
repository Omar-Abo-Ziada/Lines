using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lines.Application.Shared;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Extensions;

public static class AutoMapperExtensions
{
    public static IQueryable<TDestination> ProjectToType<TSource, TDestination>(
        this IQueryable<TSource> source, IMapper mapper)
    {
        return source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
    }
    

    public static async Task<List<TDestination>> ToListProjectedAsync<TSource, TDestination>(
        this IQueryable<TSource> source, IMapper mapper, CancellationToken cancellationToken = default)
    {
        return await source
            .ProjectTo<TDestination>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public static async Task<TDestination?> FirstOrDefaultProjectedAsync<TSource, TDestination>(
        this IQueryable<TSource> source, IMapper mapper, CancellationToken cancellationToken = default)
    {
        return await source
            .ProjectTo<TDestination>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public static PagingDto<TDestination> MapPaging<TSource, TDestination>(
            this PagingDto<TSource> source, IMapper mapper)
    {
        return new PagingDto<TDestination>(
            mapper.Map<List<TDestination>>(source.Items),
            source.TotalCount,
            source.PageNumber,
            source.PageSize
        );
    }


}