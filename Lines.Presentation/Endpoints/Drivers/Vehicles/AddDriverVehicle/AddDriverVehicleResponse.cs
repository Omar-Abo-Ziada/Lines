using Lines.Application.Features.Vehicles.AddDriverVehicle.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.AddDriverVehicle;

public record AddDriverVehicleResponse(
    Guid VehicleId,
    string Name,
    string LicensePlate,
    string Model,
    int Year,
    string VehicleTypeName,
    List<string> ImageUrls,
    bool IsPrimary,
    bool IsActive,
    bool IsVerified,
    string Status,
    decimal KmPrice,
    string? Color
);

public class AddDriverVehicleResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddDriverVehicleResponseDto, AddDriverVehicleResponse>()
            .Map(dest => dest.VehicleId, src => src.VehicleId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.LicensePlate, src => src.LicensePlate)
            .Map(dest => dest.Model, src => src.Model)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.VehicleTypeName, src => src.VehicleTypeName)
            .Map(dest => dest.ImageUrls, src => src.ImageUrls)
            .Map(dest => dest.IsPrimary, src => src.IsPrimary)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.IsVerified, src => src.IsVerified)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.KmPrice, src => src.KmPrice)
            .Map(dest => dest.Color, src => src.Color);
    }
}
