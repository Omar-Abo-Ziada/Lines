using Lines.Application.Features.Vehicles.AddDriverVehicle.DTOs;
using Lines.Application.Features.Vehicles.AddDriverVehicle.Commands;

namespace Lines.Application.Features.Vehicles.AddDriverVehicle.Orchestrators;

public record AddDriverVehicleOrchestrator(Guid userId, AddDriverVehicleDto vehicleDto) : IRequest<Result<AddDriverVehicleResponseDto>>;

public class AddDriverVehicleOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<AddDriverVehicleOrchestrator, Result<AddDriverVehicleResponseDto>>(parameters)
{
    public async override Task<Result<AddDriverVehicleResponseDto>> Handle(
        AddDriverVehicleOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddDriverVehicleCommand(request.userId, request.vehicleDto), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
