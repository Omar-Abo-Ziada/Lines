using Lines.Application.Common;
using Lines.Application.Features.Cities.Commands;
using Lines.Application.Features.Cities.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Cities.Orchestrator;

public record CreateCityOrchestrator(string Name, double Latitude, double Longitude, List<Guid> VehicleTypes) : IRequest<Result<CreateCityDto>>;
public class CreateCityOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<CreateCityOrchestrator, Result<CreateCityDto>>(parameters)
{
    public override async Task<Result<CreateCityDto>> Handle(CreateCityOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateCityCommand(request.Name, request.Latitude, request.Longitude ), cancellationToken).ConfigureAwait(false);
        if(request.VehicleTypes != null && request.VehicleTypes.Any())
        {
            foreach (var vehicleTypeId in request.VehicleTypes)
            {
                var res = await _mediator.Send(new AddCityVehicleTypeCommand(result.Id, vehicleTypeId), cancellationToken).ConfigureAwait(false);
            }
        }
        return Result<CreateCityDto>.Success(result);
    }
}