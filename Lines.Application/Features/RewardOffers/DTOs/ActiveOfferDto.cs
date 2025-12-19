namespace Lines.Application.Features.RewardOffers.DTOs;

public class ActiveOfferDto
{
    public Guid ActivationId { get; set; }
    public Guid OfferId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal ServiceFeePercent { get; set; }
    public DateTime ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int RemainingDays { get; set; }
    public bool IsActive { get; set; }
}

