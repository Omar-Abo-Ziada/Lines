using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Drivers;

public class DriverServiceFeeOffer : BaseModel
{
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public decimal ServiceFeePercent { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public bool IsGloballyActive { get; set; }

    // Navigation property for activations
    public virtual ICollection<DriverOfferActivation> Activations { get; set; }

    // Constructor
    public DriverServiceFeeOffer(Guid driverId, string title, string description, decimal price, 
        int durationDays, decimal serviceFeePercent, DateTime validFrom, DateTime validUntil, bool isGloballyActive = true)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("DriverId must be valid.", nameof(driverId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        if (durationDays <= 0)
            throw new ArgumentException("Duration must be at least 1 day.", nameof(durationDays));
        if (serviceFeePercent < 0 || serviceFeePercent > 100)
            throw new ArgumentException("Service fee percent must be between 0 and 100.", nameof(serviceFeePercent));
        if (validFrom >= validUntil)
            throw new ArgumentException("ValidFrom must be before ValidUntil.", nameof(validFrom));

        DriverId = driverId;
        Title = title;
        Description = description;
        Price = price;
        DurationDays = durationDays;
        ServiceFeePercent = serviceFeePercent;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        IsGloballyActive = isGloballyActive;
        Activations = new List<DriverOfferActivation>();
    }

    // Just for data seeding
    public DriverServiceFeeOffer()
    {
        Activations = new List<DriverOfferActivation>();
    }

    // Business Methods
    public bool IsActive()
    {
        var now = DateTime.UtcNow;
        return IsGloballyActive && now >= ValidFrom && now <= ValidUntil;
    }

    public bool IsUpcoming()
    {
        var now = DateTime.UtcNow;
        return now < ValidFrom;
    }

    public bool IsExpired()
    {
        var now = DateTime.UtcNow;
        return now > ValidUntil;
    }

    public bool IsAvailableForPurchase()
    {
        return IsActive() && IsGloballyActive;
    }
}


