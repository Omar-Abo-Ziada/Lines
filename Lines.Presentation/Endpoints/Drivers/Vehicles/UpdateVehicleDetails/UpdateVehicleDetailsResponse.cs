using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.UpdateVehicleDetails;

public record UpdateVehicleDetailsResponse(
    Guid VehicleId,
    string Name,
    string LicensePlate,
    string Model,
    int Year,
    string? Brand,
    string? Color,
    decimal KmPrice,
    bool IsActive,
    bool IsPrimary,
    bool IsVerified,
    string Status,
    List<UpdateVehiclePhotoResponse> VehiclePhotos,
    UpdateLicenseInfoResponse? LicenseInfo,
    List<string> RegistrationDocuments,
    UpdateVehicleTypeResponse VehicleType
);

public record UpdateVehiclePhotoResponse(
    string PhotoUrl,
    string? Description,
    bool IsPrimary
);

public record UpdateLicenseInfoResponse(
    string LicenseNumber,
    DateTime IssueDate,
    DateTime ExpiryDate,
    bool IsValid,
    List<string> Photos
);

public record UpdateVehicleTypeResponse(
    Guid Id,
    string Name,
    string? IconUrl
);

public class UpdateVehicleDetailsResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<VehicleDetailsDto, UpdateVehicleDetailsResponse>()
            .Map(dest => dest.VehicleId, src => src.VehicleId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.LicensePlate, src => src.LicensePlate)
            .Map(dest => dest.Model, src => src.Model)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.Brand, src => src.Brand)
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.KmPrice, src => src.KmPrice)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.IsPrimary, src => src.IsPrimary)
            .Map(dest => dest.IsVerified, src => src.IsVerified)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.VehiclePhotos, src => src.VehiclePhotos)
            .Map(dest => dest.LicenseInfo, src => src.LicenseInfo)
            .Map(dest => dest.RegistrationDocuments, src => src.RegistrationDocuments)
            .Map(dest => dest.VehicleType, src => src.VehicleType);

        config.NewConfig<VehiclePhotoDto, UpdateVehiclePhotoResponse>()
            .Map(dest => dest.PhotoUrl, src => src.PhotoUrl)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.IsPrimary, src => src.IsPrimary);

        config.NewConfig<LicenseInfoDto, UpdateLicenseInfoResponse>()
            .Map(dest => dest.LicenseNumber, src => src.LicenseNumber)
            .Map(dest => dest.IssueDate, src => src.IssueDate)
            .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
            .Map(dest => dest.IsValid, src => src.IsValid)
            .Map(dest => dest.Photos, src => src.Photos);

        config.NewConfig<VehicleTypeDto, UpdateVehicleTypeResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.IconUrl, src => src.IconUrl);
    }
}
