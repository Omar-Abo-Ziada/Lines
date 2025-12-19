using Lines.Application.Features.Vehicles.ToggleVehicleActive.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.ToggleVehicleActive;

public record ToggleVehicleActiveResponse(
    Guid VehicleId,
    bool IsActive,
    string Message
);

public class ToggleVehicleActiveResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<VehicleActiveStatusDto, ToggleVehicleActiveResponse>()
            .Map(dest => dest.VehicleId, src => src.VehicleId)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.Message, src => src.Message);
    }
}
