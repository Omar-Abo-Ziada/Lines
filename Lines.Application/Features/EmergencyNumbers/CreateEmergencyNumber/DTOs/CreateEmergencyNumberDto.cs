using AutoMapper;
using Lines.Domain.Models.Users;
using Mapster;

namespace Lines.Application.Features.EmergencyNumbers;

public class CreateEmergencyNumberDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int EmergencyNumberType { get; set; }
}

public class CreateEmergencyNumberDtoMapper : Profile
{
    public CreateEmergencyNumberDtoMapper()
    {
        CreateMap<EmergencyNumber, CreateEmergencyNumberDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.EmergencyNumberType, opt => opt.MapFrom(src => src.EmergencyNumberType));
    }
}


public class CreateEmergencyNumberDtoMappingConfg : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmergencyNumber, CreateEmergencyNumberDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.EmergencyNumberType, src => (int)src.EmergencyNumberType);
    }
}