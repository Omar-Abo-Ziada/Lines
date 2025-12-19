using Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Dtos;
using Mapster;

namespace Lines.Presentation.Endpoints.VehicleTypes
{
    public record GetVehicleTypeByKmPriceResponse
    (
        Guid Id,
        string Name,
        int Capacity,
        decimal PerKmCharge,
        decimal PerMinuteDelayCharge
    );


    public class GetVehicleTypeByKmPriceResponseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GetVehicleTypeByPriceDto, GetVehicleTypeByKmPriceResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Capacity, src => src.Capacity)
                .Map(dest => dest.PerKmCharge, src => src.PerKmCharge)
                .Map(dest => dest.PerMinuteDelayCharge, src => src.PerMinuteDelayCharge);
        }
    }
}
