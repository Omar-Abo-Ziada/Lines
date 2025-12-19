namespace Lines.Presentation.Endpoints.RewardOffers.GetAvailableOffers;

public class GetAvailableOffersResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public decimal ServiceFeePercent { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
}

