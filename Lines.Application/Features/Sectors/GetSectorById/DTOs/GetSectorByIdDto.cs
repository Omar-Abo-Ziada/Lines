using Lines.Domain.Models.Sites;
using Mapster;

namespace Lines.Application.Features.Sectors.DTOs;

public class GetSectorByIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CityId { get; set; }
    public string CityName { get; set; }
}

public class GetSectorByIdDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Sector, GetSectorByIdDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CityId, src => src.CityId)
            .Map(dest => dest.CityName, src => src.City.Name);
    }
}