using FluentValidation;

namespace Lines.Presentation.Endpoints.Passengers.DeletePassenger
{
    public record DeletePassengerRequest(Guid Id);

    public class DeletePassengerRequestValidator : AbstractValidator<DeletePassengerRequest>
    {
        public DeletePassengerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }
}
