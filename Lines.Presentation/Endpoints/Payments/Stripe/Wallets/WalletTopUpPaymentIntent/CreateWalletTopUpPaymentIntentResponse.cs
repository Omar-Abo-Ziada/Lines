namespace Lines.Presentation.Endpoints.Payments.Stripe.Wallets.WalletTopUpPaymentIntent
{
    public class CreateWalletTopUpPaymentIntentResponse
    {
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public string Currency { get; set; }
    }

}
