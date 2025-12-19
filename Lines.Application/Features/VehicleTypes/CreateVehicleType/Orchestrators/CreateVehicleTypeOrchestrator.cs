using Lines.Application.Common;
using Lines.Application.Features.VehicleTypes.Commands;
using Lines.Application.Features.VehicleTypes.Common.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.VehicleTypes.Orchestrators;

public record CreateVehicleTypeOrchestrator(
    string Name,
    string Desc,
    int Capacity,
    decimal PerKmCharge,
    decimal AverageSpeedKmPerHour,
    decimal PerMinuteDelayCharge) : IRequest<Result<CreateVehicleTypeDto>>;

public class CreateVehicleTypeOrchestratorHandler : RequestHandlerBase<CreateVehicleTypeOrchestrator , Result<CreateVehicleTypeDto>>
{
    public CreateVehicleTypeOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<CreateVehicleTypeDto>> Handle(CreateVehicleTypeOrchestrator request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new CreateVehicleTypeCommand(request.Name, request.Desc, request.Capacity, request.PerKmCharge, request.PerMinuteDelayCharge , request.AverageSpeedKmPerHour), cancellationToken);
        return Result<CreateVehicleTypeDto>.Success(result);
    }

    private async Task<Result<CreateVehicleTypeDto>> ValidateRequest(CreateVehicleTypeOrchestrator request, CancellationToken cancellationToken)
    {
        var isExist = await _mediator.Send(new CheckIfVehicleTypeExistQuery(request.Name), cancellationToken).ConfigureAwait(false);
        if (isExist)
        {
            return Result<CreateVehicleTypeDto>.Failure(VehicleTypeErrors.VehicleTypeExist);
        }
        return Result<CreateVehicleTypeDto>.Success(null!);
    }
}