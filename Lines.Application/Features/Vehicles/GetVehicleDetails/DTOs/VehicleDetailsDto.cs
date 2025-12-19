using Lines.Domain.Enums;
using Mapster;

namespace Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;

public class VehicleDetailsDto
{
    public Guid VehicleId { get; set; }
    public string Name { get; set; } = string.Empty; // Vehicle nickname
    public string LicensePlate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? Brand { get; set; } // From VehicleType
    public string? Color { get; set; }
    public decimal KmPrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsVerified { get; set; }
    public VehicleRequestStatus Status { get; set; }
    
    // Vehicle Photos
    public List<VehiclePhotoDto> VehiclePhotos { get; set; } = new List<VehiclePhotoDto>();
    
    // License Information
    public LicenseInfoDto? LicenseInfo { get; set; }
    
    // Registration Documents
    public List<string> RegistrationDocuments { get; set; } = new List<string>();
    
    // Vehicle Type
    public VehicleTypeDto VehicleType { get; set; } = new VehicleTypeDto();
}

public class VehiclePhotoDto
{
    public string PhotoUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
}

public class LicenseInfoDto
{
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsValid { get; set; }
    public List<string> Photos { get; set; } = new List<string>();
}

public class VehicleTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
}

public class VehicleDetailsDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Models.Vehicles.Vehicle, VehicleDetailsDto>()
            .Map(dest => dest.VehicleId, src => src.Id)
            .Map(dest => dest.Name, src => $"My {src.Model}") // Default naming - can be customized
            .Map(dest => dest.LicensePlate, src => src.LicensePlate)
            .Map(dest => dest.Model, src => src.Model)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.Brand, src => src.VehicleType.Name) // Using VehicleType name as brand
            .Map(dest => dest.Color, src => (string?)null) // Color not in current Vehicle model
            .Map(dest => dest.KmPrice, src => src.KmPrice)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.IsPrimary, src => src.IsPrimary)
            .Map(dest => dest.IsVerified, src => src.IsVerified)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.VehiclePhotos, src => src.Photos.Select(p => new VehiclePhotoDto
            {
                PhotoUrl = p.PhotoUrl,
                Description = p.Description,
                IsPrimary = p.IsPrimary
            }).ToList())
            .Map(dest => dest.LicenseInfo, src => src.License != null ? new LicenseInfoDto
            {
                LicenseNumber = src.License.LicenseNumber,
                IssueDate = src.License.IssueDate,
                ExpiryDate = src.License.ExpiryDate,
                IsValid = src.License.IsValid,
                Photos = src.License.Photos.Select(lp => lp.PhotoUrl).ToList()
            } : null)
            .Map(dest => dest.RegistrationDocuments, src => new List<string>()) // Handled in query
            .Map(dest => dest.VehicleType, src => new VehicleTypeDto
            {
                Id = src.VehicleType.Id,
                Name = src.VehicleType.Name,
                IconUrl = (string?)null // IconUrl not in current VehicleType model
            });
    }
}
