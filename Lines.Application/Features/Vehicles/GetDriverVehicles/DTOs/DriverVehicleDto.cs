using Lines.Domain.Enums;
using Mapster;

namespace Lines.Application.Features.Vehicles.GetDriverVehicles.DTOs;

public class DriverVehicleDto
{
    public Guid VehicleId { get; set; }
    public string Name { get; set; } = string.Empty; // Vehicle nickname (e.g., "My First Car")
    public string LicensePlate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? Brand { get; set; } // From VehicleType or separate field
    public string VehicleTypeName { get; set; } = string.Empty; // e.g., "Sedan", "SUV"
    public string? ImageUrl { get; set; } // First vehicle photo
    public bool IsPrimary { get; set; } // Green checkmark indicator
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public VehicleRequestStatus Status { get; set; }
    public decimal KmPrice { get; set; }
    public string? Color { get; set; }
}

public class DriverVehicleDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Models.Vehicles.Vehicle, DriverVehicleDto>()
            .Map(dest => dest.VehicleId, src => src.Id)
            .Map(dest => dest.Name, src => $"My {src.Model}") // Default naming - can be customized
            .Map(dest => dest.LicensePlate, src => src.LicensePlate)
            .Map(dest => dest.Model, src => src.Model)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.Brand, src => src.VehicleType.Name) // Assuming VehicleType has Brand info
            .Map(dest => dest.VehicleTypeName, src => src.VehicleType.Name)
            .Map(dest => dest.ImageUrl, src => src.Photos.FirstOrDefault() != null ? src.Photos.FirstOrDefault()!.PhotoUrl : null)
            .Map(dest => dest.IsPrimary, src => src.IsPrimary)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.IsVerified, src => src.IsVerified)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.KmPrice, src => src.KmPrice)
            .Map(dest => dest.Color, src => (string?)null); // Color not in current Vehicle model
    }
}
