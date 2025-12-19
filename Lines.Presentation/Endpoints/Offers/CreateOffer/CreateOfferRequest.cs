using FluentValidation;

namespace Lines.Presentation.Endpoints.Offers.CreateOffer;

public record CreateOfferRequest(decimal estimatedPrice,
    float distanceToArriveInMeters,
    int timeToArriveInMinutes,
    Guid tripRequestId);



public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequest>
{
    public CreateOfferRequestValidator()
    {
        RuleFor(x => x.estimatedPrice)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.distanceToArriveInMeters)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.timeToArriveInMinutes)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.tripRequestId).NotEmpty();
    }
}
