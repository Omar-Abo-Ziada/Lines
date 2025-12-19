using Lines.Domain.Models.Common;
using Lines.Domain.Models.Sites;

namespace Lines.Domain.Models.Drivers;

public class DriverAddress : BaseModel
{
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    
    public Guid CityId { get; set; }
    public virtual City City { get; set; }
    
    public Guid? SectorId { get; set; } // Province/Sector
    public virtual Sector? Sector { get; set; }
    
    public string Address { get; set; } = string.Empty; // Renamed from Street
    public string PostalCode { get; set; } = string.Empty;
    public string? LimousineBadgeUrl { get; set; } // New field from screenshot
    
    public bool IsPrimary { get; set; } // For future multiple addresses support

    // Constructor
    public DriverAddress() { }

    public DriverAddress(Guid driverId, Guid cityId, string address, string postalCode, 
        Guid? sectorId = null, string? limousineBadgeUrl = null, bool isPrimary = true)
    {
        DriverId = driverId;
        CityId = cityId;
        Address = address;
        PostalCode = postalCode;
        SectorId = sectorId;
        LimousineBadgeUrl = limousineBadgeUrl;
        IsPrimary = isPrimary;
    }

    // Business Methods
    public void UpdateAddress(string address, string postalCode, Guid? sectorId = null)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty", nameof(address));
        
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

        Address = address;
        PostalCode = postalCode;
        SectorId = sectorId;
    }

    public void UpdateLimousineBadge(string? limousineBadgeUrl)
    {
        LimousineBadgeUrl = limousineBadgeUrl;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void SetAsSecondary()
    {
        IsPrimary = false;
    }
}
