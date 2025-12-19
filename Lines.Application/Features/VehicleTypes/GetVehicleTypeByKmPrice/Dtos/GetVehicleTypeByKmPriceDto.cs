using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Dtos
{
    public class GetVehicleTypeByPriceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PerKmCharge { get; set; }
        public decimal PerMinuteDelayCharge { get; set; }
    }


    public class GetVehicleTypeByKmPriceMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Models.Vehicles.VehicleType, GetVehicleTypeByPriceDto>();
                
        }
    }


}
