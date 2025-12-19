using Lines.Domain.Enums;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Drivers;

namespace Lines.Domain.Models.Vehicles;

//[Table("Vehicles", Schema = "Vehicle")]
public class Vehicle : BaseModel
{
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public bool IsVerified { get; set; } // Admin verified vehicle documents
    public VehicleRequestStatus Status { get; set; }
    public decimal KmPrice { get; set; } // Renamed from kmPrice for consistency
    public string? RegistrationDocumentUrls { get; set; } // JSON array of document URLs
    public bool IsPrimary { get; set; } // First vehicle is primary
    public bool IsActive { get; set; } = true; // For soft deletion


    public virtual ICollection<VehiclePhoto> Photos { get; set; }

    public Guid LicenseId { get; set; }
    public virtual VehicleLicense License { get; set; }

    public Guid DriverId { get; set; } // FK to Driver
    public virtual Driver Driver { get; set; }

    public Guid VehicleTypeId { get; set; } // FK to VehicleType
    public virtual VehicleType VehicleType { get; set; }


    public Vehicle(Guid driverId, Guid vehicleTypeId, string make, string model, int year, string color, string licensePlate, decimal kmPrice)
    {
        if (driverId == Guid.Empty) throw new ArgumentException("DriverId is required.", nameof(driverId));
        if (vehicleTypeId == Guid.Empty) throw new ArgumentException("VehicleTypeId is required.", nameof(vehicleTypeId));
     //   if (string.IsNullOrWhiteSpace(make)) throw new ArgumentException("Make is required.", nameof(make));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model is required.", nameof(model));
        if (year < 1900 || year > DateTime.UtcNow.Year + 2) throw new ArgumentOutOfRangeException(nameof(year), "Invalid vehicle year.");
        if (string.IsNullOrWhiteSpace(licensePlate)) throw new ArgumentException("License plate is required.", nameof(licensePlate));
        // Add more validations

        DriverId = driverId;
        VehicleTypeId = vehicleTypeId;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
        Status = VehicleRequestStatus.PendingVerification;
        IsVerified = false;
        Photos = new List<VehiclePhoto>();
        KmPrice = kmPrice;
        IsPrimary = false; // Set explicitly when creating
        IsActive = true;

        //TODO: raise event to infrastructure that notify admin we have new vehicle req to be verfied
    }


    // Just for data seeding
    public Vehicle()
    {

    }
    public void UpdateDetails(string model, int year)
    {
        // Add validation
        if (year < 1900 || year > DateTime.UtcNow.Year + 2)
            throw new ArgumentOutOfRangeException(nameof(year), "Invalid vehicle year.");

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.", nameof(model));

        Model = model;
        Year = year;
    }

    public void VerifyVehicle()
    {
        IsVerified = true;
        Status = VehicleRequestStatus.Approved;
    }

    public void SetStatus(VehicleRequestStatus newStatus)
    {
        Status = newStatus;
    }

    public void AddPhoto(VehiclePhoto photo)
    {
        if (photo == null) throw new ArgumentNullException(nameof(photo));
        Photos.Add(photo);
    }

    public void UpdateKmPrice(decimal newKmPrice)
    {
        if (newKmPrice < 0) throw new ArgumentOutOfRangeException(nameof(newKmPrice), "KmPrice cannot be negative.");
        KmPrice = newKmPrice;
    }

    public void AddLicense(VehicleLicense license)
    {
        if (license == null)
            throw new ArgumentNullException(nameof(license));
        if (License != null)
            throw new InvalidOperationException("A license has already been added to this vehicle.");
        License = license;
        LicenseId = license.Id;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void SetAsSecondary()
    {
        IsPrimary = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateRegistrationDocuments(List<string> documentUrls)
    {
        if (documentUrls == null) throw new ArgumentNullException(nameof(documentUrls));
        RegistrationDocumentUrls = System.Text.Json.JsonSerializer.Serialize(documentUrls);
    }
}