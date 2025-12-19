using Lines.Domain.Value_Objects;

namespace Lines.Application.Features.Common.DTOs;

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
    public int Order { get; set; }
}

public class LocationDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Location, LocationDto>()
            .Map(dest => dest.Latitude, src => src.Latitude)
            .Map(dest => dest.Longitude, src => src.Longitude)
            .Map(dest => dest.Address, src => src.Address);

        config.NewConfig<EndTripLocation, LocationDto>()
           .Map(dest => dest.Latitude, src => src.Location.Latitude)
           .Map(dest => dest.Longitude, src => src.Location.Longitude)
           .Map(dest => dest.Address, src => src.Location.Address);

         config.NewConfig<LocationDto, Location>()
            .Map(dest => dest.Latitude, src => src.Latitude)
            .Map(dest => dest.Longitude, src => src.Longitude)
            .Map(dest => dest.Address, src => src.Address);

         config.NewConfig<LocationDto, EndTripLocation>()
            .Map(dest => dest.Location.Latitude, src => src.Latitude)
            .Map(dest => dest.Location.Longitude, src => src.Longitude)
            .Map(dest => dest.Location.Address, src => src.Address);

    }
}