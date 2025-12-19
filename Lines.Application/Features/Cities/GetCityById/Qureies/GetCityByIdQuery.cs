using Lines.Application.Common;
using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Cities.Qureies;

public record GetCityByIdQuery(Guid Id) : IRequest<CityByIdDto>;
public class GetCityByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<City> repository)
    : RequestHandlerBase<GetCityByIdQuery, CityByIdDto>(parameters)
{
    private readonly IRepository<City>  _repository = repository;

    public override async Task<CityByIdDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<City>();
        predicate.And(city => city.Id == request.Id);
        return await _repository
            .Get(predicate)
            .ProjectToType<CityByIdDto>()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}