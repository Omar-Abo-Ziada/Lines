using Lines.Domain.Enums;

namespace Lines.Application.Features.Vehicles.AddDriverVehicle.DTOs;

public class AddDriverVehicleDto
{
    public string Name { get; set; } = string.Empty; // Vehicle nickname
    public Guid VehicleTypeId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public decimal KmPrice { get; set; }
    public string? Color { get; set; }
    public List<string> ImageUrls { get; set; } = new List<string>(); // Multiple image URLs
    public List<string> LicensePhotoUrls { get; set; } = new(); // NEW: License photo URLs
    public string LicenseNumber { get; set; } = string.Empty; // NEW: License number
    public DateTime LicenseIssueDate { get; set; } // NEW: License issue date
    public DateTime LicenseExpiryDate { get; set; } // NEW: License expiry date
}

public class AddDriverVehicleResponseDto
{
    public Guid VehicleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string VehicleTypeName { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new List<string>(); // Multiple image URLs
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public VehicleRequestStatus Status { get; set; }
    public decimal KmPrice { get; set; }
    public string? Color { get; set; }
}
