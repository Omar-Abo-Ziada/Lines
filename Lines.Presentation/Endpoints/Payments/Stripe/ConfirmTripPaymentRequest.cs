namespace Lines.Presentation.Endpoints.Payments.Stripe
{
    public class ConfirmTripPaymentRequest
    {
        public string PaymentIntentId { get; set; } = default!;
    }
}
