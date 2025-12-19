namespace Lines.Presentation.Endpoints.Payments.Stripe
{

    public class CreateTripPaymentIntentResponse
    {
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
