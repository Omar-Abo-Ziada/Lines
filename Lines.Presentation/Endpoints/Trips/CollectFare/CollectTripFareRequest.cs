using FluentValidation;

namespace Lines.Presentation.Endpoints.Trips.CollectFare;

public record CollectTripFareRequest(Guid TripId);

public class CollectTripFareRequestValidator : AbstractValidator<CollectTripFareRequest>
{
    public CollectTripFareRequestValidator()
    {
        RuleFor(x => x.TripId)
            .NotEmpty()
            .WithMessage("Trip ID is required");
    }
}
