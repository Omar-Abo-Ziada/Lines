using Lines.Domain.Models.Vehicles;
using Mapster;

namespace Lines.Application.Features.VehicleTypes.DTOs;

public class GetVehicleTypeByIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Capacity { get;  set; } 
    public decimal PerKmCharge { get;  set; }
    public decimal PerMinuteDelayCharge { get;  set; }
}

public class GetVehicleTypeByIdDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<VehicleType, GetVehicleTypeByIdDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Capacity, src => src.Capacity)
            .Map(dest => dest.PerKmCharge, src => src.PerKmCharge)
            .Map(dest => dest.PerMinuteDelayCharge, src => src.PerMinuteDelayCharge);
    }
}