using Lines.Domain.Enums;
using Mapster;

namespace Lines.Application.Features.Drivers.GetDriverProfile.DTOs;

public class DriverProfileDto
{
    // Personal Information (from Figma screenshot)
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? CommercialRegistration { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public IdentityType? IdentityType { get; set; }
    public string? PersonalPictureUrl { get; set; }
    
    // License Photos (from Figma screenshot)
    public List<string> LicensePhotoUrls { get; set; } = new List<string>();
}

public class DriverProfileDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Models.Drivers.Driver, DriverProfileDto>()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.CommercialRegistration, src => src.CommercialRegistration)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
            .Map(dest => dest.IdentityType, src => src.IdentityType)
            .Map(dest => dest.PersonalPictureUrl, src => src.PersonalPictureUrl)
            .Map(dest => dest.LicensePhotoUrls, src => src.Licenses.FirstOrDefault(l => l.IsCurrent && l.IsActive) != null ? src.Licenses.FirstOrDefault(l => l.IsCurrent && l.IsActive)!.Photos.Select(p => p.PhotoUrl).ToList() : new List<string>());
    }
}
