using Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class GetFavoriteLocationByIdResponse
{
public Guid Id { get; set; }
public string Name { get; set; }
public double Latitude { get; set; }
public double Longitude { get; set; }
}

public class GetFavoriteLocationByIdResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetFavoriteLocationByIdDto, GetFavoriteLocationByIdResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Latitude, src => src.Latitude)
            .Map(dest => dest.Longitude, src => src.Longitude);
    }
}