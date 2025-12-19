using FluentValidation;
using Lines.Domain.Enums;
using Lines.Presentation.Endpoints.TripRequests.CreateTripRequest;

namespace Lines.Presentation.Endpoints.TermsAndConditions.AddTerms
{
    public record AddTermsRequest
        (
        TermsType Type,
        string Header,
        string Content,
        int Order
        );

    public class AddTermsRequestValidator : AbstractValidator<AddTermsRequest>
    {
        public AddTermsRequestValidator()
        {

            RuleFor(x => x.Header).NotEmpty().WithMessage("Header is required.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
            RuleFor(x => x.Order).NotEmpty().WithMessage("Order type is required.");
            RuleFor(x => x.Type).NotNull().WithMessage("Type type is required.");
        }
    }
}
