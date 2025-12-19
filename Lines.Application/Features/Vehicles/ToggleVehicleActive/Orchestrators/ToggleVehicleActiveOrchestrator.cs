using Lines.Application.Features.Vehicles.ToggleVehicleActive.DTOs;
using Lines.Application.Features.Vehicles.ToggleVehicleActive.Commands;

namespace Lines.Application.Features.Vehicles.ToggleVehicleActive.Orchestrators;

public record ToggleVehicleActiveOrchestrator(Guid vehicleId, Guid userId) : IRequest<Result<VehicleActiveStatusDto>>;

public class ToggleVehicleActiveOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<ToggleVehicleActiveOrchestrator, Result<VehicleActiveStatusDto>>(parameters)
{
    public async override Task<Result<VehicleActiveStatusDto>> Handle(
        ToggleVehicleActiveOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ToggleVehicleActiveCommand(request.vehicleId, request.userId), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
