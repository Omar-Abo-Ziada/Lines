using Lines.Application.Features.Common.DTOs;
using Lines.Domain.Value_Objects;

namespace Lines.Application.Features.TripRequests.Commands;

public record CreateEndTripLocationCommand(LocationDto  Location, Guid TripRequestId) :  IRequest<Guid>;

public class CreateEndTripLocationCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<EndTripLocation> repository)

    : RequestHandlerBase<CreateEndTripLocationCommand, Guid>(parameters)
{
    private readonly IRepository<EndTripLocation> _repository = repository;

    public override async Task<Guid> Handle(CreateEndTripLocationCommand request, CancellationToken cancellationToken)
    {
        var location = new Location() {Address = request.Location.Address, Latitude = request.Location.Latitude, Longitude = request.Location.Longitude};

        var endTripLocation = new EndTripLocation(request.TripRequestId, location, request.Location.Order);
        var res= await _repository
                .AddAsync(endTripLocation,  cancellationToken);
        return res.Id;
    }
}