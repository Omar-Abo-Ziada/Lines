using Lines.Domain.Models.Vehicles;

namespace Lines.Application.Features.VehicleTypes.Commands;

public record UpdateVehicleTypeCommand(Guid Id, string Name, string Desc, int  Capacity, decimal PerKmCharge, decimal PerMinuteDelayCharge) 
    : IRequest<bool>;
public class UpdateVehicleTypeCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<UpdateVehicleTypeCommand, bool>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<bool> Handle(UpdateVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var vehicleType = await _repository.GetByIdAsync(request.Id, cancellationToken);
        
        vehicleType.Name = request.Name;
        vehicleType.Description = request.Desc;
        vehicleType.Capacity = request.Capacity;
        vehicleType.UpdatePricing(request.PerKmCharge, request.PerMinuteDelayCharge);
        
        await _repository.UpdateAsync(vehicleType, cancellationToken).ConfigureAwait(false);
        
        return true;
    }
}