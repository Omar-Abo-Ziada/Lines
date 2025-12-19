namespace Lines.Presentation.Endpoints.Payments.Stripe
{

    public class ConfirmTripPaymentResponse
    {
        public bool Success { get; set; }
        public string Status { get; set; } = default!;
    }
}
