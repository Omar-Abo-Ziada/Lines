using Lines.Application.Common;
using Lines.Application.Features.VehicleTypes.Commands;
using Lines.Application.Features.VehicleTypes.Common.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.VehicleTypes.Orchestrators;

public record GetAllVehicleTypesOrchestrator(string? Name , int? Capacity, int PageSize, int PageNumber) : IRequest<Result<PagingDto<GetAllVehicleTypesDto>>>;

public class GetAllVehicleTypesOrchestratorHandler : RequestHandlerBase<GetAllVehicleTypesOrchestrator , Result<PagingDto<GetAllVehicleTypesDto>>>
{
    public GetAllVehicleTypesOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<PagingDto<GetAllVehicleTypesDto>>> Handle(GetAllVehicleTypesOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllVehicleTypesQuery(request.Name , request.Capacity, request.PageSize, request.PageNumber), cancellationToken);
        return Result<PagingDto<GetAllVehicleTypesDto>>.Success(result);
    }
}