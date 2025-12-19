using Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.DTOs;

namespace Lines.Application.Interfaces.Stripe
{
    public interface IPaymentGateway
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, string description);
        Task<PaymentConfirmationResult> ConfirmPaymentAsync(string paymentIntentId);
        Task<RefundResult> RefundAsync(string chargeId, decimal? amount = null);
        Task<string> CreateCustomerAsync(string name, string email);  ///TODO: should be edited
        Task<string> CreatePaymentMethodIntent(string customerId, PaymentMethodType type);
        Task AttachPaymentMethodToCustomerAsync(string paymentMethodId, string customerId);
        Task DetachPaymentMethodFromCustomerAsync(string paymentMethodId);
        Task<string> CreateAccountOnboardingLinkAsync(string accountId, string refreshUrl, string returnUrl, CancellationToken ct);
        Task<string> CreateExpressConnectedAccountAsync(Guid driverId, CancellationToken ct);
        List<PaymentGatewayCreditCardDto>? GetCreditCardsByCustomerId(string customerId);
        Task SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId, CancellationToken cancellationToken = default);
        string GetCustomerDefaultPaymentMethodIdByCustomerId(string customerId);

    }

}



