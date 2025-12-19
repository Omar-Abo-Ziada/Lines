namespace Lines.Presentation.Endpoints.Offers.Accept_Offer;

public record AcceptOfferRequest(Guid OfferId);

public class AcceptOfferRequestValidator : AbstractValidator<AcceptOfferRequest>
{
    public AcceptOfferRequestValidator()
    {
        RuleFor(x => x.OfferId).NotEmpty();
    }
}