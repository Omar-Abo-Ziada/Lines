using Lines.Domain.Models.Sites;
using Mapster;

namespace Lines.Application.Features.Cities.DTOs;

public class CityByIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class CityByIdDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<City, CityByIdDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);
    }
}