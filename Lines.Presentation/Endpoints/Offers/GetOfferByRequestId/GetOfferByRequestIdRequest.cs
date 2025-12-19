using FluentValidation;
using Lines.Presentation.Endpoints.Sectors;

namespace Lines.Presentation.Endpoints.Offers.GetOfferByRequestId
{
    public record GetOfferByRequestIdRequest(Guid RequestId);
    public class GetOfferByRequestIdRequestValidator : AbstractValidator<GetOfferByRequestIdRequest>
    {
        public GetOfferByRequestIdRequestValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty().WithMessage("Id is required");
        }
    }

}
