using Lines.Application.Features.Trips.CollectTripFare.Commands;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Trips.CollectTripFare.Orchestrators;

public record CollectTripFareOrchestrator(Guid TripId, Guid DriverId) : IRequest<Result<bool>>;

public class CollectTripFareOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository)
    : RequestHandlerBase<CollectTripFareOrchestrator, Result<bool>>(parameters)
{
    public async override Task<Result<bool>> Handle(CollectTripFareOrchestrator request, CancellationToken cancellationToken)
    {
        // Validate trip exists
        var tripExists = await repository.Get()
            .AnyAsync(t => t.Id == request.TripId, cancellationToken);

        if (!tripExists)
        {
            return Result<bool>.Failure(TripErrors.TripNotFound);
        }

        // Execute command
        var result = await _mediator.Send(new CollectTripFareCommand(request.TripId, request.DriverId), cancellationToken);

        return result;
    }
}
