namespace Lines.Application.Features.TripRequests.Commands;

public record UpdateEndTripLocationWithTripCommand(Guid TripRequestId, Guid TripId) : IRequest<bool>;
public class UpdateEndTripLocationWithTripCommandHandler : RequestHandlerBase<UpdateEndTripLocationWithTripCommand, bool>
{
    private readonly IRepository<EndTripLocation> _repository;
    public UpdateEndTripLocationWithTripCommandHandler(RequestHandlerBaseParameters parameters, IRepository<EndTripLocation> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<bool> Handle(UpdateEndTripLocationWithTripCommand request, CancellationToken cancellationToken)
    {
        var endTripLocations = _repository.Get(x => x.TripRequestId == request.TripRequestId);

        if (endTripLocations == null || !endTripLocations.Any())
        {
            return false;
        }

        foreach (var endTripLocation in endTripLocations)
        {
            endTripLocation.TripId = request.TripId;  // not assigned
        }

        await _repository.UpdateRangeAsync(endTripLocations, cancellationToken);

        return true;
    }
}