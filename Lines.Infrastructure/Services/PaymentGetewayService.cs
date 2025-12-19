//using Braintree;
//using Lines.Application.Features.PaymentMethods.Common.DTOs;
//using Microsoft.Extensions.Configuration;
//using Lines.Application.Interfaces;
//using Lines.Domain.Enums;
//using Stripe;

//namespace Lines.Infrastructure.Services;

//public class PaymentGetewayService : IPaymentGetewayService
//{
//    private readonly IBraintreeGateway _gateway;

//    public PaymentGetewayService(IConfiguration configuration)
//    {
//        var braintreeConfig = configuration.GetSection("Braintree");
//        _gateway = new BraintreeGateway(
//            braintreeConfig["Environment"],
//            braintreeConfig["MerchantId"],
//            braintreeConfig["PublicKey"],
//            braintreeConfig["PrivateKey"]);
//    }
//    public async Task<(bool Succeeded, string CustomerId, string Errors)> CreateCustomer(Guid Id, string firstName, string lastName, string email)
//    {
//        var customerRequest = new CustomerRequest
//        {
//            Id = Id.ToString(),
//            FirstName = firstName,
//            LastName = lastName,
//            Email = email
//        };
//        Result<Customer> result = await _gateway.Customer.CreateAsync(customerRequest);
//        if (!result.IsSuccess())
//        {
//            return (false, null, result.Message);
//        }
//        return (true, result.Target.Id, null);
//    }

//    public async Task<(bool Succeeded, PaymentMethodDto PaymentMethod, string Errors)> CreatePaymentMethod(string customerId, string nonce)
//    {
//        var request = new PaymentMethodRequest
//        {
//            CustomerId = customerId,
//            PaymentMethodNonce = nonce
//        };
//        Result<PaymentMethod> result = await _gateway.PaymentMethod.CreateAsync(request);
//        if (!result.IsSuccess())
//        {
//            return (false, null, result.Message);
//        }
//        var token = result.Target.Token;
//        var type = GetPaymentMethodType(result.Target);
//        var details = GetPaymentMethodDetails(result.Target);
//        var brand = GetPaymentMethodBrand(result.Target);
//        var (expirationYear, expirationMonth) = GetExpirationDate(result.Target);
//        var paymentMethod = new PaymentMethodDto(token, type, details, brand, expirationYear, expirationMonth);
//        return (true, paymentMethod, null);
//    }

//    public Task<(bool Succeeded, string Token, string Errors)> CreateTransaction(Guid UserId, Guid paymentMethodId, decimal amount)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<string> GenerateClientToken(string customerId)
//    {
//        var clientTokenRequest = new ClientTokenRequest();
//        if (!string.IsNullOrEmpty(customerId))
//        {
//            clientTokenRequest.CustomerId = customerId;
//        }
//        return Task.FromResult(_gateway.ClientToken.Generate(clientTokenRequest));
//    }

//    private PaymentMethodType GetPaymentMethodType(Braintree.PaymentMethod paymentMethod)
//    {
//        if (paymentMethod is CreditCard) return PaymentMethodType.card;
//        if (paymentMethod is PayPalAccount) return PaymentMethodType.Paypal;
//        if (paymentMethod is ApplePayCard) return PaymentMethodType.ApplePay;
//        if (paymentMethod is AndroidPayCard) return PaymentMethodType.GooglePay;
//        return PaymentMethodType.Cash;
//    }
//    private string GetPaymentMethodDetails(Braintree.PaymentMethod paymentMethod)
//    {
//        if (paymentMethod is CreditCard card) return $"**** **** **** {card.LastFour}";
//        if (paymentMethod is PayPalAccount payPal) return payPal.Email;
//        return string.Empty;
//    }

//    private string? GetPaymentMethodBrand(PaymentMethod paymentMethod)
//    {
//        return paymentMethod switch
//        {
//            CreditCard creditCard => creditCard.CardType switch
//            {

//                CreditCardCardType.MASTER_CARD => "MasterCard",
//                CreditCardCardType.VISA => "VISA",
//                _ => "Unknown"
//            },
//            ApplePayCard applePayCard => applePayCard.CardType,
//            AndroidPayCard androidPayCard => androidPayCard.CardType,
//            _ => "Unknown"
//        };
//    }

//    private (int Year, int Month) GetExpirationDate(Braintree.PaymentMethod paymentMethod)
//    {
//        return paymentMethod switch
//        {
//            CreditCard creditCard => (int.Parse(creditCard.ExpirationYear), int.Parse(creditCard.ExpirationMonth)),

//            ApplePayCard applePayCard => (int.Parse(applePayCard.ExpirationYear), int.Parse(applePayCard.ExpirationMonth)),

//            AndroidPayCard androidPayCard => (int.Parse(androidPayCard.ExpirationYear), int.Parse(androidPayCard.ExpirationMonth)),

//            _ => (0, 0) // No expiration for PayPal, Venmo, Bank accounts
//        };
//    }

//}