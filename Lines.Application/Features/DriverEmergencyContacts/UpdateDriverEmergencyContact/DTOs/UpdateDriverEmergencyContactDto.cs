using AutoMapper;
using Lines.Domain.Models.Drivers;
using Mapster;

namespace Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.DTOs;

public class UpdateDriverEmergencyContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}

public class UpdateDriverEmergencyContactDtoMapper : Profile
{
    public UpdateDriverEmergencyContactDtoMapper()
    {
        CreateMap<DriverEmergencyContact, UpdateDriverEmergencyContactDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
    }
}

public class UpdateDriverEmergencyContactDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverEmergencyContact, UpdateDriverEmergencyContactDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);
    }
}

