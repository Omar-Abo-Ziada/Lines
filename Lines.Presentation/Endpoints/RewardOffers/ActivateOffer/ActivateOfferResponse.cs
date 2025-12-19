namespace Lines.Presentation.Endpoints.RewardOffers.ActivateOffer;

public class ActivateOfferResponse
{
    public Guid ActivationId { get; set; }
    public Guid OfferId { get; set; }
    public string OfferTitle { get; set; }
    public DateTime ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal NewWalletBalance { get; set; }
    public string PaymentReference { get; set; }
}

