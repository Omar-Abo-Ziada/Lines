using AutoMapper;
using Lines.Application.Features.EmergencyNumbers;
using Mapster;

namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public record EditEmergencyNumberResponse(Guid Id, string Name, string PhoneNumber, int EmergencyNumberType);

public class EditEmergencyNumberResponseMapper : Profile
{
    public EditEmergencyNumberResponseMapper()
    {
        CreateMap<EditEmergencyNumberDto, EditEmergencyNumberResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.EmergencyNumberType, opt => opt.MapFrom(src => (int)src.EmergencyNumberType));
    }
}

public class EditEmergencyNumberResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EditEmergencyNumberDto, EditEmergencyNumberResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.EmergencyNumberType, src => (int)src.EmergencyNumberType);
    }
}