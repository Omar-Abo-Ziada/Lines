using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Models.Vehicles;

namespace Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Dtos
{
    public class GetAllVehicleTypesWithExpectedPriceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal PerKmCharge { get; set; }
        public decimal AverageSpeedKmPerHour { get; set; }

        public decimal ExpectedPrice { get; set; }
        public decimal? ExpectedPriceAfterDiscount { get; set; }

        public int EstimatedTimeInMinutes { get; set; }

    }

    public class GetallVehicleTypesWithExpectedPriceDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<VehicleType, GetAllVehicleTypesWithExpectedPriceDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Capacity, src => src.Capacity)
            .Map(dest => dest.PerKmCharge, src => src.PerKmCharge)
            .Map(dest => dest.AverageSpeedKmPerHour, src => src.AverageSpeedKmPerHour);


        }
    }
}
