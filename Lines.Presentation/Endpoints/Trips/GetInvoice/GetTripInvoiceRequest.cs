using FluentValidation;

namespace Lines.Presentation.Endpoints.Trips.GetInvoice;

public record GetTripInvoiceRequest(Guid TripId);

public class GetTripInvoiceRequestValidator : AbstractValidator<GetTripInvoiceRequest>
{
    public GetTripInvoiceRequestValidator()
    {
        RuleFor(x => x.TripId)
            .NotEmpty()
            .WithMessage("Trip ID is required");
    }
}
