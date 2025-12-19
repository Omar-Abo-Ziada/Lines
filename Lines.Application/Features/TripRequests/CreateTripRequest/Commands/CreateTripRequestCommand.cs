using Lines.Application.Features.TripRequests.DTOs;

namespace Lines.Application.Features.TripRequests.Commands;

public record CreateTripRequestCommand
    (
     TripRequest tripRequest
    ) : IRequest<CreateTripRequestDto>;

public class CreateTripRequestCommandHandler(RequestHandlerBaseParameters parameters, IRepository<TripRequest> repository)
    : RequestHandlerBase<CreateTripRequestCommand, CreateTripRequestDto>(parameters)
{
    private readonly IRepository<TripRequest> _repository = repository;

    public override async Task<CreateTripRequestDto> Handle(CreateTripRequestCommand request, CancellationToken cancellationToken)
    {

        //var tripRequest = new TripRequest(request.TripRequest.PassengerId, request.TripRequest.StartLocation, request.VehicleTypeId, 
        //    request.PaymentMethodId, request.IsScheduled, request.ScheduledAt, request.EstimatedPrice, request.Distance, request.endLocations);
        
        var res = await _repository.AddAsync(request.tripRequest, cancellationToken);
        return res.Adapt<CreateTripRequestDto>();
    }
}