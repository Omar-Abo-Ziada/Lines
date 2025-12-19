using Lines.Application.Features.Sectors.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Sectors;

public record GetAllSectorsResponse(Guid Id, string Name, Guid CityId, string CityName);
public class GetAllSectorsResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetAllSectorsDto, GetAllSectorsResponse>()
            .Map(dest => dest.CityId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CityId, src => src.CityId)
            .Map(dest => dest.CityName, src => src.CityName);
    }
}