using Lines.Domain.Models.Users;

namespace Lines.Application.Features.EmergencyNumbers.Shared.DTOs;

public class GetEmergencyNumberDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int EmergencyNumberType { get; set; }
}

public class GetEmergencyNumberDtoMapper : AutoMapper.Profile
{
    public GetEmergencyNumberDtoMapper()
    {
        CreateMap<EmergencyNumber, GetEmergencyNumberDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.EmergencyNumberType, opt => opt.MapFrom(src => (int)src.EmergencyNumberType));
    }
}
public class GetEmergencyNumberDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmergencyNumber, GetEmergencyNumberDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.EmergencyNumberType, src => (int)src.EmergencyNumberType);
    }
}
