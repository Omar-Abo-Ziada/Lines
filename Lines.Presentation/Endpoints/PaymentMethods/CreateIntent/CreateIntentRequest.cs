using Lines.Domain.Enums;

namespace Lines.Presentation.Endpoints.PaymentMethods.CreateIntent
{
    public record CreateIntentRequest(PaymentMethodType paymentMethodType);

    public class CreateIntentRequestValidator : AbstractValidator<CreateIntentRequest>
    {
        public CreateIntentRequestValidator()
        {
            RuleFor(r => r.paymentMethodType)
                .IsInEnum()
                .WithMessage("Invalid payment method type.");
        }
    }

}
