using Lines.Domain.Models.Drivers;
using Mapster;

namespace Lines.Application.Features.DriverEmergencyContacts.Shared.DTOs;

public class DriverEmergencyContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}

public class DriverEmergencyContactDtoMapper : AutoMapper.Profile
{
    public DriverEmergencyContactDtoMapper()
    {
        CreateMap<DriverEmergencyContact, DriverEmergencyContactDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
    }
}

public class DriverEmergencyContactDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverEmergencyContact, DriverEmergencyContactDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);
    }
}

