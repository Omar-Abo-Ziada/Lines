namespace Lines.Application.Features.TripRequests.Command;

public record CancelTripRequestCommand(Guid TripRequestId, string CancellationReason) : IRequest<Unit>; 
public class CancelTripRequestCommandHandler : RequestHandlerBase<CancelTripRequestCommand, Unit>
{
    private readonly IRepository<TripRequest> _repository;
    public CancelTripRequestCommandHandler(RequestHandlerBaseParameters parameters, IRepository<TripRequest> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Unit> Handle(CancelTripRequestCommand request, CancellationToken cancellationToken)
    { 
        var tripRequest = await _repository.GetByIdAsync(request.TripRequestId, cancellationToken);
        tripRequest.CancelRequestByPassenger(request.CancellationReason);
        await _repository.UpdateAsync(tripRequest, cancellationToken);
        return Unit.Value;
    }
}