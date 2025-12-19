using Lines.Application.Features.Vehicles.GetDriverVehicles.DTOs;
using Lines.Application.Features.Vehicles.GetDriverVehicles.Queries;

namespace Lines.Application.Features.Vehicles.GetDriverVehicles.Orchestrators;

public record GetDriverVehiclesOrchestrator(Guid userId) : IRequest<Result<List<DriverVehicleDto>>>;

public class GetDriverVehiclesOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetDriverVehiclesOrchestrator, Result<List<DriverVehicleDto>>>(parameters)
{
    public async override Task<Result<List<DriverVehicleDto>>> Handle(
        GetDriverVehiclesOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDriverVehiclesQuery(request.userId), cancellationToken)
            .ConfigureAwait(false);

        return result;
    }
}
