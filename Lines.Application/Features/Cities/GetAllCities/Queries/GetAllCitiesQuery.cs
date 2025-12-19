using Application.Common.Helpers;
using Lines.Application.Features.Cities.DTOs;
using Lines.Domain.Models.Sites;
using LinqKit;

namespace Lines.Application.Features.Cities.Queries;

public record GetAllCitiesQuery(string? Name, int PageSize, int PageNumber) : IRequest<PagingDto<GetAllCitiesDto>>;
public class GetAllCitiesQueryHandler(RequestHandlerBaseParameters parameters, IRepository<City> repository)
    : RequestHandlerBase<GetAllCitiesQuery, PagingDto<GetAllCitiesDto>>(parameters)
{
    private readonly IRepository<City> _repository = repository;

    public override async Task<PagingDto<GetAllCitiesDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<City>(true);
        if (!string.IsNullOrEmpty(request.Name))
        {
            predicate = predicate.And(x => x.Name.Contains(request.Name));
        }

        var query = _repository
            .Get(predicate)
            .ProjectToType<GetAllCitiesDto>();
        var result = await PagingHelper.CreateAsync(source: query, pageSize: request.PageSize, pageNumber: request.PageNumber, cancellationToken: cancellationToken);
        return result;
    }
}