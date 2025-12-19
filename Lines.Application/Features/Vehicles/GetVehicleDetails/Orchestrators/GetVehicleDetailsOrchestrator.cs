using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;
using Lines.Application.Features.Vehicles.GetVehicleDetails.Queries;

namespace Lines.Application.Features.Vehicles.GetVehicleDetails.Orchestrators;

public record GetVehicleDetailsOrchestrator(Guid vehicleId, Guid userId) : IRequest<Result<VehicleDetailsDto>>;

public class GetVehicleDetailsOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetVehicleDetailsOrchestrator, Result<VehicleDetailsDto>>(parameters)
{
    public async override Task<Result<VehicleDetailsDto>> Handle(
        GetVehicleDetailsOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleDetailsQuery(request.vehicleId, request.userId), cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            return Error.NullValue;
        }

        return result;
    }
}
