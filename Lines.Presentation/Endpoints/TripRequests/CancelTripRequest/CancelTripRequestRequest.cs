using FluentValidation;

namespace Lines.Presentation.Endpoints.TripRequests;

public record CancelTripRequestRequest
(
    Guid TripRequestId,
    string CancellationReason
);

public class CancelTripRequestRequestValidator : AbstractValidator<CancelTripRequestRequest>
{
    public CancelTripRequestRequestValidator()
    {
        RuleFor(x => x.TripRequestId).NotEmpty();
        RuleFor(x => x.CancellationReason).NotEmpty()
                                          .MaximumLength(500);
    }
}