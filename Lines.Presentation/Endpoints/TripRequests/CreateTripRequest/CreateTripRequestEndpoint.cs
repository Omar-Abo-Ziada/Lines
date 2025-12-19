using Lines.Application.Features.TripRequests.Orchestrator;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Lines.Presentation.Endpoints.Notifications.GetNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TripRequests.CreateTripRequest;

public class CreateTripRequestEndpoint : BaseController<CreateTripRequestRequest, bool>
{
    private BaseControllerParams<CreateTripRequestRequest> _dependencyCollection;
    public CreateTripRequestEndpoint(BaseControllerParams<CreateTripRequestRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    
    [HttpPost("trip-request/create")]
    [Authorize]
    public async Task<ApiResponse<bool>> Create([FromBody] CreateTripRequestRequest request)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var userId = GetCurrentUserId();
        var passengerId = GetCurrentPassengerId();

        if (userId == Guid.Empty)
        {
            return HandleResult<CreateTripRequestRequest, bool>(
                Result<CreateTripRequestRequest>.Failure(new Error(Code: "UNAUTHORIZED", Description: "User not authenticated", Type: ErrorType.Validation)));
        }

        var res = await _mediator.Send(new CreateTripRequestOrchestrator(
            passengerId, 
            request.StartLocation,
            request.EndLocations,
            request.IsScheduled,
            request.ScheduledAt,
            request.VehicleTypeId,
            request.PaymentMethodId,
            request.EstimatedPrice,
            request.Distance,
            userId,
            request.UserRewardId)).ConfigureAwait(false);
        
        return HandleResult<bool, bool>(res);
    }
    
}