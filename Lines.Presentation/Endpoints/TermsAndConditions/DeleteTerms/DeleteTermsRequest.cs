using FluentValidation;
using Lines.Domain.Enums;
using Lines.Presentation.Endpoints.TripRequests.CreateTripRequest;

namespace Lines.Presentation.Endpoints.TermsAndConditions.DeleteTerms
{
    public record DeleteTermsRequest
        (
        Guid Id
       
        );

    public class DeleteTermsRequestValidator : AbstractValidator<DeleteTermsRequest>
    {
        public DeleteTermsRequestValidator()
        {

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
      
        }
    }
}
