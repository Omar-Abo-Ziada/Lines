using Lines.Application.Features.Sectors.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Sectors;

public record CreateSectorResponse(Guid Id, string Name, Guid CityId, string CityName);
public class CreateSectorResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSectorDto, CreateSectorResponse>()
            .Map(dest => dest.CityId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CityId, src => src.CityId)
            .Map(dest => dest.CityName, src => src.CityName);
    }
}