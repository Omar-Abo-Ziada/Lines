using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetDriversNotificationOptionByDriverIds.Queries;
using Lines.Application.Features.DriverBlockedPassengers.GetBlockedPassengerIds.Queries;
using Lines.Application.Features.Vehicles.GetVehicleKmPricesByDriverIds.Queries;
using Lines.Application.Features.Vehicles.GetVehicleTypeIdByDriverId.Queries;
using Lines.Application.Interfaces;
using MediatR;

namespace Lines.Application.Features.Drivers.GetNearbyDriverConnectionsFilteredByKmPrice.Orchestrators
{
    public record GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
        double latitude, 
        double longitude, 
        decimal kmPrice, 
        Guid vehicleTypeId, 
        Guid passengerId,
        bool isAnonymous = false,
        double radiusKm = 10.0) 
                    : IRequest<List<string>>;


    public class GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestratorHandler
                            : RequestHandlerBase<GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator, List<string>>
    {
        private IDriverConnectionService _driverConnectionService;

        public GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestratorHandler(IDriverConnectionService driverConnectionService, RequestHandlerBaseParameters parameters)
             : base(parameters)
        {
            _driverConnectionService = driverConnectionService;
        }

        public override async Task<List<string>> Handle(GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator request, CancellationToken cancellationToken)
        {
            var nearbyDrivers = await _driverConnectionService
                                .GetNearbyDriversAsync(request.latitude, request.longitude, request.radiusKm);

            var driverIds = nearbyDrivers.Select(d => d.DriverId).ToList();

            var connectionIds = new List<string>();

            // First, filter drivers by vehicle type
            var vehicleTypeIds = await _mediator.Send(new GetVehicleTypeIdsByDriverIdsQuery(driverIds));

            var driversWithMatchingVehicleType = vehicleTypeIds
                .Where(v => v.VehicleTypeId == request.vehicleTypeId)
                .Select(v => v.DriverId)
                .ToList();
          
            if(driversWithMatchingVehicleType.Any())
            {

                // Filter nearby drivers to only include those with matching vehicle type
                var filteredNearbyDrivers = nearbyDrivers
                .Where(d => driversWithMatchingVehicleType.Contains(d.DriverId))
                .ToList();

                var filteredDriverIds = filteredNearbyDrivers.Select(d => d.DriverId).ToList();

                // Filter out drivers who have blocked this passenger (skip for anonymous trips)
                if (!request.isAnonymous && request.passengerId != Guid.Empty)
                {
                    var driversWhoBlockedPassenger = await _mediator.Send(
                        new GetBlockedPassengerIdsByDriverIdsQuery(filteredDriverIds, request.passengerId),
                        cancellationToken);

                    if (driversWhoBlockedPassenger.Any())
                    {
                        // Exclude blocked drivers from the filtered list
                        filteredNearbyDrivers = filteredNearbyDrivers
                            .Where(d => !driversWhoBlockedPassenger.Contains(d.DriverId))
                            .ToList();
                        
                        filteredDriverIds = filteredNearbyDrivers.Select(d => d.DriverId).ToList();
                    }
                }

                // If no drivers left after blocking filter, return empty list
                if (!filteredDriverIds.Any())
                {
                    return connectionIds;
                }

                // retrieve all flags in 1 query instead of looping all drivers and make queries by number by drivers
                var driversNotificationOption = await _mediator.Send(new GetDriversNotificationOptionByDriverIdsQuery
                                                                    (filteredDriverIds));

                driversNotificationOption.Where(nd => !nd.IsNotifiedForOnlyTripsAboveMyPrice)
                     .ToList()
                     .ForEach(nd => connectionIds.AddRange(filteredNearbyDrivers
                     .Where(d => d.DriverId == nd.DriverId).SelectMany(d => d.ConnectionIds)));

                // for those wanna be notified for optimized price only 
                var driversNotifiedForOnlyTripsAboveTheirPriceIds = driversNotificationOption
                                                            .Where(d => d.IsNotifiedForOnlyTripsAboveMyPrice)
                                                            .Select(d => d.DriverId)
                                                            .ToList();

                if (driversNotifiedForOnlyTripsAboveTheirPriceIds.Any())
                {
                    var vehicleKmPrices = await _mediator.Send
                                            (new GetVehicleKmPricesByDriverIdsQuery(driversNotifiedForOnlyTripsAboveTheirPriceIds));

                    vehicleKmPrices.Where(v => v.KmPrice <= request.kmPrice)
                        .ToList()
                        .ForEach(v => connectionIds.AddRange(filteredNearbyDrivers
                        .Where(nd => nd.DriverId == v.DriverId).SelectMany(nd => nd.ConnectionIds)));
                }
            }

            return connectionIds;
        }
    }

}
