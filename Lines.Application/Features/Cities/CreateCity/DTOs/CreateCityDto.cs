using Mapster;

namespace Lines.Application.Features.Cities.DTOs;

public class CreateCityDto
{
    public Guid Id { get; set; }
    public string Name  { get; set; }
}

public class CreateCityDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCityDto, CreateCityDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);
    }
}