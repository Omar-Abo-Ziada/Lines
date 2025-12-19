using FluentValidation;

namespace Lines.Presentation.Endpoints.Trips.CompleteTrip
{
    public record CompleteTripRequest(Guid tripId);
    
    public class CompleteTripRequestValidator : AbstractValidator<CompleteTripRequest>
    {
        public CompleteTripRequestValidator()
        {
            RuleFor(x => x.tripId).NotEmpty()
                .WithMessage("Trip ID is required.");
        }
    }
}
