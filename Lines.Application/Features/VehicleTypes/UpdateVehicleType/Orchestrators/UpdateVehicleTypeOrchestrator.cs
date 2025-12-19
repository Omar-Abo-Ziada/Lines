using Lines.Application.Common;
using Lines.Application.Features.VehicleTypes.Commands;
using Lines.Application.Features.VehicleTypes.Common.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.VehicleTypes.Orchestrators;

public record UpdateVehicleTypeOrchestrator(
    Guid Id,
    string Name,
    string Desc,
    int Capacity,
    decimal PerKmCharge,
    decimal PerMinuteDelayCharge) : IRequest<Result<bool>>;

public class UpdateVehicleTypeOrchestratorHandler : RequestHandlerBase<UpdateVehicleTypeOrchestrator , Result<bool>>
{
    public UpdateVehicleTypeOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<bool>> Handle(UpdateVehicleTypeOrchestrator request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new UpdateVehicleTypeCommand(request.Id, request.Name, request.Desc, request.Capacity, request.PerKmCharge, request.PerMinuteDelayCharge), cancellationToken);
        return Result<bool>.Success(result);
    }

    private async Task<Result<bool>> ValidateRequest(UpdateVehicleTypeOrchestrator request, CancellationToken cancellationToken)
    {
        var isExist = await _mediator.Send(new CheckIfVehicleTypeExistQuery(request.Name, request.Id), cancellationToken).ConfigureAwait(false);
        if (isExist)
        {
            return Result<bool>.Failure(VehicleTypeErrors.VehicleTypeExist);
        }
        return Result<bool>.Success(true);
    }
}