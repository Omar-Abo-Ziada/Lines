

using FluentValidation;

namespace Lines.Presentation.Endpoints.TripRequests.EditTripRequestPrice
{
    // ?? Request Model
    public record EditTripRequestPriceRequest(
        Guid TripRequestId,
        decimal NewPrice
    );

    // ?? Fluent Validation
    public class EditTripRequestPriceRequestValidator : AbstractValidator<EditTripRequestPriceRequest>
    {
        public EditTripRequestPriceRequestValidator()
        {
            RuleFor(x => x.TripRequestId)
                .NotEmpty().WithMessage("Trip request ID is required.");

            RuleFor(x => x.NewPrice)
                .GreaterThan(0).WithMessage("New price must be greater than zero.");
        }
    }
}



//using FluentValidation;
//using Lines.Application.Features.Common.DTOs;

//namespace Lines.Presentation.Endpoints.TripRequests.EditTripRequestPrice;

//public record EditTripRequestPriceRequest
//(
//  Guid Id,
//    decimal EstimatedPrice
//);

//public class EditTripRequestPriceRequestValidator : AbstractValidator<EditTripRequestPriceRequest>
//{
//    public EditTripRequestPriceRequestValidator()
//    {
//        RuleFor(x => x.EstimatedPrice).NotEmpty().WithMessage("Price is required.");
//        RuleFor(x => x.Id).NotEmpty().WithMessage("Id  is required.");
//    }
//}