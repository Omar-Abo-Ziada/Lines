using Lines.Application.Features.Cities.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Cities;

public record CreateCityResponse(Guid Id, string Name);
public class CreateCityResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCityDto, CreateCityResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);;
    }
}