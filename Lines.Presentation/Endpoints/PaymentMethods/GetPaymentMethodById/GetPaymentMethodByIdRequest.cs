namespace Lines.Presentation.Endpoints.PaymentMethods.GetPaymentMethodById;

public record GetPaymentMethodByIdRequest(Guid Id);

public class GetPaymentMethodByIdRequestValidator : AbstractValidator<GetPaymentMethodByIdRequest>
{
    public GetPaymentMethodByIdRequestValidator()
    {
        RuleFor(src => src.Id).NotEmpty().WithMessage("Id cannot be empty");
    }
}