using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Drivers;

public class DriverOfferActivation : BaseModel
{
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    
    public Guid OfferId { get; set; }
    public virtual DriverServiceFeeOffer Offer { get; set; }
    
    public DateTime ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public string PaymentReference { get; set; }

    // Constructor
    public DriverOfferActivation(Guid driverId, Guid offerId, DateTime activationDate, 
        DateTime expirationDate, string paymentReference)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("DriverId must be valid.", nameof(driverId));
        if (offerId == Guid.Empty)
            throw new ArgumentException("OfferId must be valid.", nameof(offerId));
        if (activationDate >= expirationDate)
            throw new ArgumentException("Activation date must be before expiration date.", nameof(activationDate));
        if (string.IsNullOrWhiteSpace(paymentReference))
            throw new ArgumentException("Payment reference is required.", nameof(paymentReference));

        DriverId = driverId;
        OfferId = offerId;
        ActivationDate = activationDate;
        ExpirationDate = expirationDate;
        PaymentReference = paymentReference;
        IsActive = true;
    }

    // Just for data seeding
    public DriverOfferActivation()
    {
    }

    // Business Methods
    public bool HasExpired()
    {
        return DateTime.UtcNow > ExpirationDate;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public int RemainingDays()
    {
        if (HasExpired())
            return 0;
        
        return (int)(ExpirationDate - DateTime.UtcNow).TotalDays;
    }
}

