using Lines.Application.Features.FavoriteLocations.Queries;
using Lines.Application.Features.FavoriteLocations.Shared.DTOs;

namespace Lines.Application.Features.TripRequests.Orchestrator;

public record GetAllFavoritePlacesByPassengerIdOrchestrator(Guid PassengerId) : IRequest<Result<List<GetAllFavoriteLocations>>>;

public class GetAllFavoritePlacesByPassengerIdOrchestratorHandler(RequestHandlerBaseParameters parameters) 
            : RequestHandlerBase<GetAllFavoritePlacesByPassengerIdOrchestrator, Result<List<GetAllFavoriteLocations>>>(parameters)
{
    public override async Task<Result<List<GetAllFavoriteLocations>>> Handle(GetAllFavoritePlacesByPassengerIdOrchestrator request, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new GetAllFavoritePlacesByPassengerIdQuery(request.PassengerId), cancellationToken)
                                    .ConfigureAwait(false);

        return Result<List<GetAllFavoriteLocations>>.Success(result.Value);
    }
}