using Lines.Domain.Enums;

namespace Lines.Presentation.Endpoints.PaymentMethods.CreatePaymentMethod
{
    public record CreatePaymentMethodRequest(string PaymentMethodId, PaymentMethodType paymentMethodType, bool isDefault = false);

    public class CreatePaymentMethodRequestValidator : AbstractValidator<CreatePaymentMethodRequest>
    {
        public CreatePaymentMethodRequestValidator()
        {
            RuleFor(x => x.PaymentMethodId).NotEmpty().WithMessage("Payment Method Id is required.");
            RuleFor(x => x.paymentMethodType).NotEmpty().WithMessage("Payment Method Type is required.");
        }
    }

}
