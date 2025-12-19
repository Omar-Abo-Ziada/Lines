using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Models.Vehicles;

namespace Lines.Application.Features.VehicleTypes.Commands;

public record CreateVehicleTypeCommand(string Name, string Desc, int  Capacity, decimal PerKmCharge, decimal PerMinuteDelayCharge , decimal AverageSpeedKmPerHour) : IRequest<CreateVehicleTypeDto>;
public class CreateVehicleTypeCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<CreateVehicleTypeCommand, CreateVehicleTypeDto>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<CreateVehicleTypeDto> Handle(CreateVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new VehicleType(request.Name, request.Desc, request.Capacity, request.PerKmCharge, request.PerMinuteDelayCharge ,request.AverageSpeedKmPerHour);
        var res = await _repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return res.Adapt<CreateVehicleTypeDto>();
    }
}