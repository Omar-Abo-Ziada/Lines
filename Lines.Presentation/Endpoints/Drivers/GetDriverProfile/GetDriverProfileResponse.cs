using Lines.Application.Features.Drivers.GetDriverProfile.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverProfile;

public record GetDriverProfileResponse(
    // Personal Information (from Figma screenshot)
    string FirstName,
    string LastName,
    string? CompanyName,
    string? CommercialRegistration,
    DateTime? DateOfBirth,
    string? IdentityType,
    string? PersonalPictureUrl,
    
    // License Photos (from Figma screenshot)
    List<string> LicensePhotoUrls
);

public class GetDriverProfileResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverProfileDto, GetDriverProfileResponse>()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.CommercialRegistration, src => src.CommercialRegistration)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
            .Map(dest => dest.IdentityType, src => src.IdentityType.HasValue ? src.IdentityType.Value.ToString() : null)
            .Map(dest => dest.PersonalPictureUrl, src => src.PersonalPictureUrl)
            .Map(dest => dest.LicensePhotoUrls, src => src.LicensePhotoUrls);
    }
}
