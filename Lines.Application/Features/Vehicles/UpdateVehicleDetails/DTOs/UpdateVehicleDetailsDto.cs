using Lines.Domain.Enums;

namespace Lines.Application.Features.Vehicles.UpdateVehicleDetails.DTOs;

public class UpdateVehicleDetailsDto
{
    // Basic vehicle fields (optional - only update if provided)
    public string? Name { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public string? Color { get; set; }
    public decimal? KmPrice { get; set; }
    public string? LicensePlate { get; set; }
    public Guid? VehicleTypeId { get; set; }
    
    // File URLs (populated after upload)
    public List<string> ImageUrls { get; set; } = new List<string>();
    public List<string> LicensePhotoUrls { get; set; } = new List<string>();
    public List<string> RegistrationDocumentUrls { get; set; } = new List<string>();
}
