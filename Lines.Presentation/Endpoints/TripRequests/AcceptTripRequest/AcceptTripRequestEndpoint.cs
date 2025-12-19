using Lines.Application.Features.TripRequests;
using Lines.Application.Features.TripRequests.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Lines.Presentation.Endpoints.TripRequests.CreateTripRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TripRequests;

public class AcceptTripRequestEndpoint : BaseController<AcceptTripRequestRequest, AcceptTripRequestResponse>
{
    private BaseControllerParams<AcceptTripRequestRequest> _dependencyCollection;
    public AcceptTripRequestEndpoint(BaseControllerParams<AcceptTripRequestRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [Authorize]
    [HttpPut("trip-request/accept")]
    public async Task<ApiResponse<AcceptTripRequestResponse>> Accept([FromBody] AcceptTripRequestRequest request)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<AcceptTripRequestRequest, AcceptTripRequestResponse>(
                Result<AcceptTripRequestRequest>.Failure(new Error(Code: "UNAUTHORIZED", Description: "User not authenticated", Type: ErrorType.Validation)));
        }

        var res = await _mediator.Send(new AcceptTripRequestOrchestrator(request.TripRequestId, userId)).ConfigureAwait(false);
        
        return HandleResult<CreatedTripForDriverDto, AcceptTripRequestResponse>(res);
    }

}