using AutoMapper;
using Lines.Domain.Models.Users;
using Mapster;

namespace Lines.Application.Features.EmergencyNumbers;

public class EditEmergencyNumberDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public EmergencyNumberType EmergencyNumberType { get; set; }
}

public class EditEmergencyNumberDtoMapper : Profile
{
    public EditEmergencyNumberDtoMapper()
    {
        CreateMap<EmergencyNumber, EditEmergencyNumberDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.EmergencyNumberType, opt => opt.MapFrom(src => src.EmergencyNumberType));
    }
}


public class EditEmergencyNumberDtoMappingConfg : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmergencyNumber, EditEmergencyNumberDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.EmergencyNumberType, src => (int)src.EmergencyNumberType);
    }
}