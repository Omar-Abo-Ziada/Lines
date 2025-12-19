using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Dtos;
using Mapster;

namespace Lines.Presentation.Endpoints.VehicleTypes.GetAllVehicleTypesWithExpectedPrice
{
    public record GetAllVehicleTypesWithExpectedPriceResponse
        (Guid Id, string Name,  int Capacity, decimal ExpectedPrice, decimal? ExpectedPriceAfterDiscount
        , int EstimatedTimeInMinutes);

    //: IRegister
    public class GetAllVehicleTypesWithExpectedPriceResponseMappingConfig
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GetAllVehicleTypesWithExpectedPriceDto, GetAllVehicleTypesWithExpectedPriceResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Capacity, src => src.Capacity)
                .Map(dest => dest.EstimatedTimeInMinutes, src => src.EstimatedTimeInMinutes)
                .Map(dest => dest.ExpectedPrice, src => src.ExpectedPrice)
                .Map(dest => dest.ExpectedPriceAfterDiscount, src => src.ExpectedPriceAfterDiscount);
        }
    }
}
