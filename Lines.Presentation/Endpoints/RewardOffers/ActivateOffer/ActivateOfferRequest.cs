using FluentValidation;

namespace Lines.Presentation.Endpoints.RewardOffers.ActivateOffer;

public record ActivateOfferRequest(Guid OfferId);

public class ActivateOfferRequestValidator : AbstractValidator<ActivateOfferRequest>
{
    public ActivateOfferRequestValidator()
    {
        RuleFor(x => x.OfferId)
            .NotEmpty()
            .WithMessage("Offer ID is required.");
    }
}

