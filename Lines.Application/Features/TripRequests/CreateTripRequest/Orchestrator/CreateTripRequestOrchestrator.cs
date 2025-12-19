using Lines.Application.Features.Common.DTOs;
using Lines.Application.Features.FCMNotifications.Queries;
using Lines.Application.Features.Notifications.AddNotifications.Orchestrator;
using Lines.Application.Features.Rewards.GetRewardById.Orchestrators;
using Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators;
using Lines.Application.Features.Wallets.TripWalletPayment.Commands;
using Lines.Application.Helpers;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Features.TripRequests.Orchestrator;

public record CreateTripRequestOrchestrator(
    Guid PassengerId,
    LocationDto StartLocation,
    List<LocationDto> EndLocations,
    bool IsScheduled,
    DateTime? ScheduledAt,
    Guid VehicleTypeId,
    Guid PaymentMethodId,
    decimal EstimatedPrice,
    double Distance,
    Guid UserId,
    Guid? UserRewardId
) : IRequest<Result<bool>>;


public class CreateTripRequestOrchestratorHandler
    : RequestHandlerBase<CreateTripRequestOrchestrator, Result<bool>>
{
    private readonly IHubContext<DriverHub> _hubContext;
    private readonly IDriverConnectionService _driverConnectionService;
    private readonly IFcmNotifier _notifier;
    private readonly IApplicationUserService _appUserService;
    private readonly ILogger<PayTripWithWalletCommandHandler> _logger;
    public CreateTripRequestOrchestratorHandler(
        RequestHandlerBaseParameters parameters,
        IHubContext<DriverHub> hubContext,
        IDriverConnectionService driverConnectionService,
        IFcmNotifier notifier
,
        IApplicationUserService appUserService,
        ILogger<PayTripWithWalletCommandHandler> logger) : base(parameters)
    {
        _hubContext = hubContext;
        _driverConnectionService = driverConnectionService;
        _notifier = notifier;
        _appUserService = appUserService;
        _logger = logger;
    }

    public override async Task<Result<bool>> Handle(CreateTripRequestOrchestrator request, CancellationToken cancellationToken)
    {
        // ================================================================
        // STEP 1 — Validate price + build TripRequest object
        // ================================================================
        var vehicleType = await _mediator.Send(new GetVehicleTypeByIdOrchestrator(request.VehicleTypeId));

        // get userRewardById 
        decimal? fareAfterRewardApplied = null;
        if (request.UserRewardId != null)
        {
            var userReward = _mediator.Send(new GetUserRewardByIdOrchestrator((Guid)request.UserRewardId));
            if(userReward.Result.Value != null) 
            {
               bool isValid = userReward.Result.Value.IsValid(request.UserId);
               if(!isValid)
                {
                    return TripRequestErrors.InvalidUserReward;
                }

               // get reward by id 
               var reward = _mediator.Send(new GetRewardByIdOrchestrator(userReward.Result.Value.RewardId));
                if (reward.Result.Value != null)
                {
                    // apply discount to estimated price 
                   fareAfterRewardApplied = FareAfterRewardAppliedCalculator.Calculate(request.EstimatedPrice , reward.Result.Value.DiscountPercentage , reward.Result.Value.MaxValue);
                }
            }
        }

        var startLocation = request.StartLocation.Adapt<Location>();
        var endLocations = request.EndLocations.Adapt<List<EndTripLocation>>();


        TripRequest tripRequest = new()
        {
            PassengerId = request.PassengerId,
            StartLocation = request.StartLocation.Adapt<Location>(),
            EndLocations = request.EndLocations.Adapt<List<EndTripLocation>>(),
            IsScheduled = request.IsScheduled,
            ScheduledAt = request.ScheduledAt,
            VehicleTypeId = request.VehicleTypeId,
            PaymentMethodId = request.PaymentMethodId,
            EstimatedPrice = request.EstimatedPrice,
            Distance = request.Distance,
            UserRewardId = request.UserRewardId
        };


        var isValidPrice = tripRequest.IsValidPrice(vehicleType.Value.PerKmCharge);
        if (!isValidPrice)
        {
            var minPrice = tripRequest.CalculateMinPrice(vehicleType.Value.PerKmCharge);
            return TripRequestErrors.InvalidPrice($"Price cannot be lower than {minPrice}.");
        }

        // ================================================================
        // STEP 2 — Create TripRequest in DB
        // ================================================================
        var tripRequestDto = await _mediator.Send(new CreateTripRequestCommand(tripRequest), cancellationToken);

        decimal kmPrice = request.EstimatedPrice / (decimal)request.Distance;

        // ================================================================
        // STEP 3 — Notify passenger (always)
        // ================================================================
        #region Resolve UserIds

        var passengerUserId = await _appUserService
            .GetUserIdByPassengerIdAsync(request.PassengerId);

        if (passengerUserId == null || passengerUserId == Guid.Empty)
        {
            _logger.LogError("No ApplicationUser found for PassengerId {PassengerId}", request.PassengerId);
            return Result<bool>.Success(true);
        }

        #endregion


        await _notifier.SendToUserAsync(
            passengerUserId.Value,
            NotificationTemplates.TripCreatedTitle,
            NotificationTemplates.TripCreatedBody,
            new { TripRequestId = tripRequestDto.Id });

        // Save Notification
        await _mediator.Send(new AddNotificationsOrchestrator(
            passengerUserId.Value,
            NotificationTemplates.TripCreatedTitle,
            NotificationTemplates.TripCreatedBody,
            NotificationType.TripRequestCreated));

        // ================================================================
        // STEP 4 — Fetch nearby drivers in 10km
        // ================================================================
        var nearbyConnections = await _mediator.Send(
            new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
                tripRequestDto.StartLocation.Latitude,
                tripRequestDto.StartLocation.Longitude,
                kmPrice,
                tripRequestDto.VehicleTypeId,
                request.PassengerId,
                isAnonymous: false,
                radiusKm: 10.0));

        // Extract driverIds
        var nearbyDriverIds = _driverConnectionService.GetDriverIdsFromConnections(nearbyConnections);

        // ================================================================
        // STEP 5 — Send SignalR to Online Drivers
        // ================================================================
        if (nearbyConnections.Any())
        {
            await _hubContext.Clients.Clients(nearbyConnections)
                .SendAsync("NewTripRequest", tripRequestDto, fareAfterRewardApplied ,cancellationToken);  
        }

        // ================================================================
        // STEP 6 — Send FCM to Offline Drivers ONLY + Save Notification (DB)
        // ================================================================
        var tokens = await _mediator.Send(new GetFcmTokensByUserIdsQuery(nearbyDriverIds.ToList()));

        if (tokens.Any())
        {
            await _notifier.SendToManyAsync(tokens,
                NotificationTemplates.NewTripForDriverTitle,
                NotificationTemplates.NewTripForDriverBody,
                new { TripRequestId = tripRequestDto.Id, FareAfterRewardApplied = fareAfterRewardApplied });
        }


        var driverUserIdsMap = await _appUserService.GetUserIdsByDriverIdsAsync(nearbyDriverIds);



        // Save Notification (DB)
        foreach (var driverId in nearbyDriverIds)
        {

            if (!driverUserIdsMap.TryGetValue(driverId, out var driverUserId))
            {
                _logger.LogWarning("No ApplicationUser found for DriverId {DriverId}", driverId);
                continue;
            }

            await _mediator.Send(new AddNotificationsOrchestrator(
                driverUserId,
                NotificationTemplates.NewTripForDriverTitle,
                NotificationTemplates.NewTripForDriverBody,
                NotificationType.TripRequestCreated));
        }

        // ================================================================
        // STEP 7 — If no drivers in 10km → expand to 20km
        // ================================================================
        if (!nearbyConnections.Any())
        {
            var expandedConnections = await _mediator.Send(
                new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
                    tripRequestDto.StartLocation.Latitude,
                    tripRequestDto.StartLocation.Longitude,
                    kmPrice,
                    tripRequestDto.VehicleTypeId,
                    request.PassengerId,
                    isAnonymous: false,
                    radiusKm: 20.0));

            var expandedDriverIds = _driverConnectionService.GetDriverIdsFromConnections(expandedConnections);
            var expandedTokens = await _mediator.Send(new GetFcmTokensByUserIdsQuery(expandedDriverIds.ToList()));

            // SignalR
            if (expandedConnections.Any())
            {
                await _hubContext.Clients.Clients(expandedConnections)
                    .SendAsync("NewTripRequest", tripRequestDto, fareAfterRewardApplied, cancellationToken);
            }

            // FCM + DB
            if (expandedTokens.Any())
            {
                await _notifier.SendToManyAsync(expandedTokens,
                    NotificationTemplates.NewTripForDriverTitle,
                    NotificationTemplates.NewTripForDriverBody,
                    new { TripRequestId = tripRequestDto.Id, FareAfterRewardApplied = fareAfterRewardApplied });
            }

            var expandedMap = await _appUserService.GetUserIdsByDriverIdsAsync(expandedDriverIds);

            // Save Notification
            foreach (var driverId in expandedDriverIds)
            {
                if (!expandedMap.TryGetValue(driverId, out var driverUserId))
                    continue;

                await _mediator.Send(new AddNotificationsOrchestrator(
                    driverUserId,
                    NotificationTemplates.NewTripForDriverTitle,
                    NotificationTemplates.NewTripForDriverBody,
                    NotificationType.TripRequestCreated));
            }

            // ================================================================
            // STEP 8 — If STILL no drivers → notify passenger - FCM + DB
            // ================================================================
            if (!expandedConnections.Any() && !nearbyConnections.Any())
            {
                await _notifier.SendToUserAsync(
            passengerUserId.Value,
                    NotificationTemplates.NoDriversTitle,
                    NotificationTemplates.NoDriversBody,
                    new { TripRequestId = tripRequestDto.Id, FareAfterRewardApplied = fareAfterRewardApplied });

                await _mediator.Send(new AddNotificationsOrchestrator(
                    //request.PassengerId,
                                passengerUserId.Value,

                    NotificationTemplates.NoDriversTitle,
                    NotificationTemplates.NoDriversBody,
                    NotificationType.TripRequestCreated));
            }
        }

        // ================================================================
        // DONE
        // ================================================================
        return Result<bool>.Success(true);
    }
}

#region Old Code 

//using Lines.Application.Features.Common.DTOs;

//namespace Lines.Application.Features.TripRequests.Orchestrator;

//public record CreateTripRequestOrchestrator(
//    Guid PassengerId,
//    LocationDto StartLocation,
//    List<LocationDto> EndLocations,
//    bool IsScheduled,
//    DateTime? ScheduledAt,
//    Guid VehicleTypeId,
//    Guid PaymentMethodId,
//    decimal EstimatedPrice,
//    double Distance
//) : IRequest<Result<bool>>;

//public class CreateTripRequestOrchestratorHandler : RequestHandlerBase<CreateTripRequestOrchestrator, Result<bool>>
//{
//    private IHubContext<DriverHub> _hubContext;
//    private IDriverConnectionService _driverConnectionService;
//    public CreateTripRequestOrchestratorHandler
//        (
//            RequestHandlerBaseParameters parameters,
//            IHubContext<DriverHub> hubContext,
//            IDriverConnectionService driverConnectionService
//        ) : base(parameters)
//    {
//        _hubContext = hubContext;
//        _driverConnectionService = driverConnectionService;
//    }

//    public override async Task<Result<bool>> Handle(CreateTripRequestOrchestrator request, CancellationToken cancellationToken)
//    {

//        var vehicleType = await _mediator.Send(new GetVehicleTypeByIdOrchestrator(request.VehicleTypeId));

//        var startLocation = request.StartLocation.Adapt<Location>();
//        var endLocations = request.EndLocations.Adapt<List<EndTripLocation>>();

//        TripRequest tripRequest = new()
//        {
//            PassengerId = request.PassengerId,
//            StartLocation = startLocation,
//            EndLocations = endLocations,
//            IsScheduled = request.IsScheduled,
//            ScheduledAt = request.ScheduledAt,
//            VehicleTypeId = request.VehicleTypeId,
//            PaymentMethodId = request.PaymentMethodId,
//            EstimatedPrice = request.EstimatedPrice,
//            Distance = request.Distance
//        };

//        var isValidPrice = tripRequest.IsValidPrice(vehicleType.Value.PerKmCharge);
//        if (!isValidPrice)
//        {
//            var minPrice = tripRequest.CalculateMinPrice(vehicleType.Value.PerKmCharge);
//            return TripRequestErrors.InvalidPrice($"Price can not be lower than {minPrice} for this trip request.");
//        }


//        var tripRequestDto = await _mediator.Send(new CreateTripRequestCommand(
//           tripRequest), cancellationToken);

//        decimal kmPrice = request.EstimatedPrice / (decimal)request.Distance;

//        var nearbyDriverConnections = await _mediator.Send(new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
//            tripRequestDto.StartLocation.Latitude,
//            tripRequestDto.StartLocation.Longitude,
//            kmPrice,
//            tripRequestDto.VehicleTypeId,
//            request.PassengerId,
//            isAnonymous: false,
//            radiusKm: 10.0));

//        if (nearbyDriverConnections.Any())
//        {
//            // Broadcast to specific nearby drivers
//            await _hubContext.Clients.Clients(nearbyDriverConnections)
//                .SendAsync("NewTripRequest", tripRequestDto, cancellationToken);

//        }
//        else
//        {
//            // No nearby drivers found - could implement fallback logic
//            Console.WriteLine($"No nearby drivers found for trip request {tripRequestDto.Id}");

//            // Optional: Expand search radius or notify rider
//            var expandedDriverConnections = await _mediator.Send(new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
//                tripRequestDto.StartLocation.Latitude,
//                tripRequestDto.StartLocation.Longitude,
//                kmPrice,
//                tripRequestDto.VehicleTypeId,
//                request.PassengerId,
//                isAnonymous: false,
//                radiusKm: 20.0)); // Expand to 20km

//            if (expandedDriverConnections.Any())
//            {
//                await _hubContext.Clients.Clients(expandedDriverConnections)
//                    .SendAsync("NewTripRequest", tripRequestDto, cancellationToken);

//            }
//        }
//        return Result<bool>.Success(true);
//    }
//}

#endregion

