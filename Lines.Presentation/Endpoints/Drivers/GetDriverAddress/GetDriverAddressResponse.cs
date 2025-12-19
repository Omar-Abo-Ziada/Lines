using Lines.Application.Features.Drivers.GetDriverAddress.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverAddress;

public record GetDriverAddressResponse(string Address, Guid CityId, string CityName, Guid? SectorId, string? SectorName, string PostalCode);

public class GetDriverAddressResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverAddressDto, GetDriverAddressResponse>()
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.CityId, src => src.CityId)
            .Map(dest => dest.CityName, src => src.CityName)
            .Map(dest => dest.SectorId, src => src.SectorId)
            .Map(dest => dest.SectorName, src => src.SectorName)
            .Map(dest => dest.PostalCode, src => src.PostalCode);
    }
}
