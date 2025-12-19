using FluentValidation;
using Lines.Domain.Enums;
using Lines.Presentation.Endpoints.TripRequests.CreateTripRequest;

namespace Lines.Presentation.Endpoints.TermsAndConditions.EditTerms
{
    public record EditTermsRequest
        (
        Guid Id,
        string Header,
        string Content,
        int Order
        );

    public class EditTermsRequestValidator : AbstractValidator<EditTermsRequest>
    {
        public EditTermsRequestValidator()
        {

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.Header).NotEmpty().WithMessage("Header is required.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
            RuleFor(x => x.Order).NotEmpty().WithMessage("Order type is required.");
        }
    }
}
