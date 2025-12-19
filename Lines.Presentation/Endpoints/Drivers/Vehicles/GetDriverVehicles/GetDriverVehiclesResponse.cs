using Lines.Application.Features.Vehicles.GetDriverVehicles.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.GetDriverVehicles;

public record GetDriverVehiclesResponse(
    List<DriverVehicleDto> Vehicles
);

public class GetDriverVehiclesResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<List<DriverVehicleDto>, GetDriverVehiclesResponse>()
            .Map(dest => dest.Vehicles, src => src);
    }
}
