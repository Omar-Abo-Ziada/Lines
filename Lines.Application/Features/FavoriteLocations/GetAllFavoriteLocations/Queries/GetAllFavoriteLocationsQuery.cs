using Lines.Application.Common;
using Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.DTOs;
using Lines.Application.Features.FavoriteLocations.Shared.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Sites;
using Lines.Domain.Shared;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.FavoriteLocations.Queries;

public record GetAllFavoriteLocationsQuery(string? Name) : IRequest<Result<List<GetAllFavoriteLocations>>>;
public class GetAllFavoriteLocationsQueryHandler : RequestHandlerBase<GetAllFavoriteLocationsQuery, Result<List<GetAllFavoriteLocations>>>
{
    private readonly IRepository<FavoriteLocation> _repository;
    public GetAllFavoriteLocationsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<FavoriteLocation> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<List<GetAllFavoriteLocations>>> Handle(GetAllFavoriteLocationsQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<FavoriteLocation>();
        if (!string.IsNullOrEmpty(request.Name))
        {
            predicate = predicate.And(x => x.Name.ToLower().Contains(request.Name.ToLower()));
        }
        var res = await _repository
            .Get(predicate)
            .ProjectToType<GetAllFavoriteLocations>()
            .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
            
        return Result<List<GetAllFavoriteLocations>>.Success(res);
    }
}