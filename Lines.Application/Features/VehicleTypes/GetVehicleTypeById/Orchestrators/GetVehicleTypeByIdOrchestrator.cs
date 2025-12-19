using Lines.Application.Common;
using Lines.Application.Features.VehicleTypes.Commands;
using Lines.Application.Features.VehicleTypes.Common.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.VehicleTypes.Orchestrators;

public record GetVehicleTypeByIdOrchestrator(Guid Id) : IRequest<Result<GetVehicleTypeByIdDto>>;

public class GetVehicleTypeByIdOrchestratorHandler : RequestHandlerBase<GetVehicleTypeByIdOrchestrator , Result<GetVehicleTypeByIdDto>>
{
    public GetVehicleTypeByIdOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<GetVehicleTypeByIdDto>> Handle(GetVehicleTypeByIdOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleTypeByIdQuery(request.Id), cancellationToken);
        return Result<GetVehicleTypeByIdDto>.Success(result);
    }
}