namespace Lines.Application.Features.PaymentMethods;

public static class PaymentMethodErrors
{
    public static Error CreatePaymentMethodError(string desc) => new Error("PAYMENTMETHOD.CREATE",desc,ErrorType.Validation);
    public static Error CustomerIdNotFoundError(string desc) => new Error("PAYMENTMETHOD.CustomerIdNotFoundError", desc,ErrorType.Validation);
}