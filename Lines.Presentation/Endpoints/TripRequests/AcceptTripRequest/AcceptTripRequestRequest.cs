using FluentValidation;

namespace Lines.Presentation.Endpoints.TripRequests;

public record AcceptTripRequestRequest
(
    Guid TripRequestId
);

public class AcceptTripRequestRequestValidator : AbstractValidator<AcceptTripRequestRequest>
{
    public AcceptTripRequestRequestValidator()
    {
        RuleFor(x => x.TripRequestId).NotEmpty();
    }
}