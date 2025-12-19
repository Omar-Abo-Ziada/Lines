using Lines.Application.Common;
using Lines.Application.Features.VehicleTypes.Commands;
using Lines.Application.Features.VehicleTypes.Common.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.VehicleTypes.Orchestrators;

public record DeleteVehicleTypeOrchestrator(Guid Id) : IRequest<Result<bool>>;

public class DeleteVehicleTypeOrchestratorHandler : RequestHandlerBase<DeleteVehicleTypeOrchestrator , Result<bool>>
{
    public DeleteVehicleTypeOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<bool>> Handle(DeleteVehicleTypeOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteVehicleTypeCommand(request.Id), cancellationToken);
        return Result<bool>.Success(result);
    }
}