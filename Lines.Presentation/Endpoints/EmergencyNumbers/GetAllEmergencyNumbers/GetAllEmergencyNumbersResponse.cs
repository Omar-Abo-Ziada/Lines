using AutoMapper;
using Lines.Application.Features.EmergencyNumbers.Shared.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public record GetAllEmergencyNumbersResponse(Guid Id, string Name, string PhoneNumber, int EmergencyNumberType, Guid? UserId);  

public class GetAllEmergencyNumbersResponseMapper : Profile
{
    public GetAllEmergencyNumbersResponseMapper()
    {
        CreateMap<GetEmergencyNumberDto , GetAllEmergencyNumbersResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.EmergencyNumberType, opt => opt.MapFrom(src => (int)src.EmergencyNumberType));
    }
}
public class GetAllEmergencyNumbersResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetEmergencyNumberDto, GetAllEmergencyNumbersResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.EmergencyNumberType, src => (int)src.EmergencyNumberType);
    }
}
