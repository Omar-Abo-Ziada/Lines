using Lines.Application.Features.Cities.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Cities;

public record GetCityByIdResponse(Guid Id, string Name);
public class GetCityByIdResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CityByIdDto, GetCityByIdResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);;
    }
}