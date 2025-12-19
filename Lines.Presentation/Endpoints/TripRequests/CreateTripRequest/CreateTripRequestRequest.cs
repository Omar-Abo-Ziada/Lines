using Lines.Application.Features.Common.DTOs;

namespace Lines.Presentation.Endpoints.TripRequests.CreateTripRequest;

public record CreateTripRequestRequest
(
    LocationDto StartLocation,
    List<LocationDto> EndLocations,
    bool IsScheduled,
    DateTime? ScheduledAt,
    Guid VehicleTypeId,
    Guid PaymentMethodId,   ///TODO: should be paymentMethodType "enum" >> change whenever payment logic is clear enough
    decimal EstimatedPrice,
    float Distance,
    Guid? UserRewardId
);

public class CreateTripRequestRequestValidator : AbstractValidator<CreateTripRequestRequest>
{
    public CreateTripRequestRequestValidator()
    {
        RuleFor(x => x.StartLocation).NotNull().WithMessage("StartLocation is required.");
        
        RuleFor(x => x.EndLocations)
                  .NotNull().WithMessage("EndLocations is required.")
                  .Must(list => list != null && list.Count > 0)
                  .WithMessage("At least one EndLocation is required.")
                  .Must(list => list != null && list.Count <= 5)
                  .WithMessage("You can add at most 5 EndLocations.");
        
        RuleFor(x => x.IsScheduled).NotNull().WithMessage("IsScheduled is required.");
        RuleFor(x => x.Distance).NotEmpty().WithMessage("Distance is required.");
        RuleFor(x => x.PaymentMethodId).NotEmpty().WithMessage("PaymentMethod is required.");
        RuleFor(x => x.EstimatedPrice).NotEmpty().WithMessage("Price is required.");
        RuleFor(x => x.VehicleTypeId).NotEmpty().WithMessage("Vehicle type is required.");
    }
}