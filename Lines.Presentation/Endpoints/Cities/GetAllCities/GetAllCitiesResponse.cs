using Lines.Application.Features.Cities.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Cities;

public record GetAllCitiesResponse(Guid Id, string Name);
public class GetAllCitiesResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetAllCitiesDto, GetAllCitiesResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);;
    }
}