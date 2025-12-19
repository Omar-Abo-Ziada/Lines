using Lines.Application.Features.Vehicles.UpdateVehicleDetails.DTOs;
using Lines.Application.Features.Vehicles.UpdateVehicleDetails.Commands;
using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;

namespace Lines.Application.Features.Vehicles.UpdateVehicleDetails.Orchestrators;

public record UpdateVehicleDetailsOrchestrator(Guid vehicleId, Guid userId, UpdateVehicleDetailsDto updateDto) : IRequest<Result<VehicleDetailsDto>>;

public class UpdateVehicleDetailsOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<UpdateVehicleDetailsOrchestrator, Result<VehicleDetailsDto>>(parameters)
{
    public async override Task<Result<VehicleDetailsDto>> Handle(
        UpdateVehicleDetailsOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateVehicleDetailsCommand(request.vehicleId, request.userId, request.updateDto), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
