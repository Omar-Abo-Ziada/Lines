using Lines.Application.Shared;
using Lines.Domain.Models.Trips;
using Lines.Domain.Models.Vehicles;
using Lines.Domain.Shared;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Lines.Application.Features.TripRequests.Commands;

public record EditTripRequestPriceCommand(Guid Id, decimal EstimatedPrice,Guid userId) : IRequest<Result>;

public class EditTripRequestPriceCommandHandler
    : RequestHandlerBase<EditTripRequestPriceCommand, Result>
{
    private readonly IRepository<TripRequest> _repository;
    private readonly IRepository<VehicleType> _vehicleRepository;

    public EditTripRequestPriceCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<TripRequest> repository,
        IRepository<VehicleType> vehicleRepository
    ) : base(parameters)
    {
        _repository = repository;
        _vehicleRepository = vehicleRepository;
    }

    public override async Task<Result> Handle(EditTripRequestPriceCommand request, CancellationToken cancellationToken)
    {
        var tripRequest = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tripRequest is null)
            return TripRequestErrors.TripRequestNotFound;

        if (tripRequest.PassengerId !=request.userId)
            return TripRequestErrors.NotYourTripRequest;

        if (tripRequest.Status != TripRequestStatus.Pending || tripRequest.DriverId is not null)
            return TripRequestErrors.TripRequestAcceptedOrCanceled;

        var vehicleType = await _vehicleRepository.GetByIdAsync(tripRequest.VehicleTypeId, cancellationToken);
        if (vehicleType is null)
            return Error.NotFound with { Description = "Vehicle type not found" };

        tripRequest.EstimatedPrice = request.EstimatedPrice;

        if (!tripRequest.IsValidPrice(vehicleType.PerKmCharge))
        {
            var minAllowed = tripRequest.CalculateMinPrice(vehicleType.PerKmCharge);
            var message = $"The offered price ({request.EstimatedPrice:F2}) is below the minimum allowed ({minAllowed:F2}).";
            return TripRequestErrors.InvalidPrice(message);
        }

        await _repository.UpdateAsync(tripRequest, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
