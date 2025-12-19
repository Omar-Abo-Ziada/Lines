namespace Lines.Presentation.Endpoints.Payments.Stripe.Wallets
{
    public class PayTripWithWalletResponse
    {
        public Guid TripId { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; } = "Paid";

        // اختياري
        //public decimal? NewPassengerBalance { get; set; }
    }
}
