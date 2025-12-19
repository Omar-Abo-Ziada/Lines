using Lines.Application.Common;
using Lines.Application.Features.Cities.Queries;
using Lines.Application.Features.VehicleTypes.Commands;
using Lines.Application.Features.VehicleTypes.Common.Queries;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Helpers;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.VehicleTypes.Orchestrators;

public record GetVehicleTypesListOrchestrator(double Latitude, double Longitude) : IRequest<Result<List<GetVehicleTypesListDto>>>;

public class GetVehicleTypesListOrchestratorHandler : RequestHandlerBase<GetVehicleTypesListOrchestrator , Result<List<GetVehicleTypesListDto>>>
{
    public GetVehicleTypesListOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<List<GetVehicleTypesListDto>>> Handle(GetVehicleTypesListOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleTypesListQuery(), cancellationToken);

        var citiesResponse = await _mediator.Send(new GetAllCitiesQuery(null, int.MaxValue, 1), cancellationToken);
        var cities = citiesResponse.Items;

        Guid? closestCityId = null;
        float minDistance = float.MaxValue;

        foreach (var city in cities)
        {
            var cityDistance = DistanceCalculator.CalculateDistance(
                request.Latitude, request.Longitude,
                city.Latitude, city.Longitude);

            if (cityDistance < minDistance)
            {
                minDistance = cityDistance;
                closestCityId = city.Id;
            }
        }

        if (!closestCityId.HasValue)
        {
            return result;
        }

        Guid cityId = closestCityId.Value;

        var vehicleTypesByCity = await _mediator.Send(new GetVehicleTypesByCity(cityId), cancellationToken);
        var availableVehicleTypes = new HashSet<Guid>(vehicleTypesByCity);

        foreach (var vehicle in result)
        {
            vehicle.IsAvailable = availableVehicleTypes.Contains(vehicle.Id);
        }
        
        return Result<List<GetVehicleTypesListDto>>.Success(result);
    }
}