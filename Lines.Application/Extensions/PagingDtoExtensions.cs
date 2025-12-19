using Lines.Application.Shared;
using Mapster;

namespace Lines.Application.Extensions;

public static class PagingDtoExtensions
{
    public static PagingDto<TDestination> AdaptPaging<TSource, TDestination>(  
        this PagingDto<TSource> source)
    {
        return new PagingDto<TDestination>(
            source.Items.Adapt<IEnumerable<TDestination>>(),
            source.TotalCount,
            source.PageNumber,
            source.PageSize
        );
    }
}
