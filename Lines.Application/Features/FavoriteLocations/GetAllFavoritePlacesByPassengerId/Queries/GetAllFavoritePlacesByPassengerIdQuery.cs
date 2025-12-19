using Lines.Application.Features.FavoriteLocations.Shared.DTOs;
using Lines.Domain.Models.Sites;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.FavoriteLocations.Queries;

public record GetAllFavoritePlacesByPassengerIdQuery(Guid? PassengerId) : IRequest<Result<List<GetAllFavoriteLocations>>>;
public class GetAllFavoritePlacesByPassengerIdQueryHandler : RequestHandlerBase<GetAllFavoritePlacesByPassengerIdQuery, Result<List<GetAllFavoriteLocations>>>
{
    private readonly IRepository<FavoriteLocation> _repository;
    public GetAllFavoritePlacesByPassengerIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<FavoriteLocation> repository) 
                                                  : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<List<GetAllFavoriteLocations>>> Handle(GetAllFavoritePlacesByPassengerIdQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<FavoriteLocation>();
       
        predicate = predicate.And(x => x.PassengerId == request.PassengerId);
        
        var res = await _repository
            .Get(predicate)
            .ProjectToType<GetAllFavoriteLocations>()
            .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
            
        return Result<List<GetAllFavoriteLocations>>.Success(res);
    }
}