using Lines.Domain.Models.Sites;
using Mapster;

namespace Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.DTOs;

public class GetFavoriteLocationByIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class GetFavoriteLocationByIdDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FavoriteLocation,  GetFavoriteLocationByIdDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Latitude, src => src.Latitude)
            .Map(dest => dest.Longitude, src => src.Longitude);
    }
}