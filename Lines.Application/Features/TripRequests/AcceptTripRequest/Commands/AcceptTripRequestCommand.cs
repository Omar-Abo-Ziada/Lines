namespace Lines.Application.Features.TripRequests.Commands;

public record AcceptTripRequestCommand(Guid TripRequestId, Guid DriverId) : IRequest<Unit>;

public class AcceptTripRequestCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<TripRequest> repository)
    : RequestHandlerBase<AcceptTripRequestCommand, Unit>(parameters)
{
    private readonly IRepository<TripRequest> _repository = repository;

    public override async Task<Unit> Handle(AcceptTripRequestCommand request, CancellationToken cancellationToken)
    {
        var tripRequest = await _repository.GetByIdAsync(request.TripRequestId, cancellationToken);
        tripRequest.AcceptRequest(request.DriverId, DateTime.UtcNow.AddMinutes(3));
        await _repository.UpdateAsync(tripRequest, cancellationToken);
        return Unit.Value;
    }
}