namespace Lines.Presentation.Endpoints.Payments.Stripe.Wallets.WalletTopUpPaymentIntent
{
    public class CreateWalletTopUpPaymentIntentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "QAR";
    }

}
