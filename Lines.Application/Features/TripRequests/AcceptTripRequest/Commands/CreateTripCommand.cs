using Lines.Application.Features.TripRequests.DTOs;

namespace Lines.Application.Features.TripRequests.Commands;

public record CreateTripCommand(
    Guid DriverId,
    Guid PassengerId,
    Location StartLocation,
    double Distance,
    decimal Fare,
    Guid PaymentMethodId,
    Guid TripRequestId,
    PaymentMethodType PaymentMethodType,
    decimal? FareAfterRewardApplied,
    bool IsRewardApplied = false
    ) : IRequest<CreatedTripDto>;

public class CreateTripCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository)
    : RequestHandlerBase<CreateTripCommand, CreatedTripDto>(parameters)
{
    private readonly IRepository<Trip> _repository = repository;

    public override async Task<CreatedTripDto> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        var trip = new Trip(request.DriverId, request.PassengerId, request.StartLocation, request.Distance,
            request.Fare, request.PaymentMethodId, request.TripRequestId, request.PaymentMethodType, isRewardApplied: request.IsRewardApplied,
            fareAfterRewardApplied: request.FareAfterRewardApplied);

        var result = await _repository.AddAsync(trip, cancellationToken);
        _repository.SaveChanges();
        return result.Adapt<CreatedTripDto>();
    }
}