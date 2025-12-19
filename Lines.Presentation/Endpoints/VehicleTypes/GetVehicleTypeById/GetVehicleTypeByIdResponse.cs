using Lines.Application.Features.VehicleTypes.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record GetVehicleTypeByIdResponse
    (Guid Id, string Name, string Description, int Capacity, decimal PerKmCharge, decimal PerMinuteDelayCharge);
    

public class GetVehicleTypeByIdResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetVehicleTypeByIdDto, GetVehicleTypeByIdResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Capacity, src => src.Capacity)
            .Map(dest => dest.PerKmCharge, src => src.PerKmCharge)
            .Map(dest => dest.PerMinuteDelayCharge, src => src.PerMinuteDelayCharge);
    }
}