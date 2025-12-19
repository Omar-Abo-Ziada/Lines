using Lines.Application.Features.VehicleTypes.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record GetAllVehicleTypesResponse
    (Guid Id, string Name, string Description, int Capacity, decimal PerKmCharge, decimal PerMinuteDelayCharge);
    

public class GetAllVehicleTypesResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetAllVehicleTypesDto, GetAllVehicleTypesResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Capacity, src => src.Capacity)
            .Map(dest => dest.PerKmCharge, src => src.PerKmCharge)
            .Map(dest => dest.PerMinuteDelayCharge, src => src.PerMinuteDelayCharge);
    }
}