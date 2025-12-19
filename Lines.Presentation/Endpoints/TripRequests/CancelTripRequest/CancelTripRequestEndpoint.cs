using Lines.Application.Features.TripRequests;
using Lines.Application.Features.TripRequests.DTOs;
using Lines.Application.Features.TripRequests.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TripRequests;

public class CancelTripRequestEndpoint : BaseController<CancelTripRequestRequest, bool>
{
    private BaseControllerParams<CancelTripRequestRequest> _dependencyCollection;
    public CancelTripRequestEndpoint(BaseControllerParams<CancelTripRequestRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("trip-request/cancel")]
    public async Task<ApiResponse<bool>> Cancel([FromBody] CancelTripRequestRequest request)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }

        var passengerId = GetCurrentPassengerId();
        var res = await _mediator.Send(new CancelTripRequestOrchestrator(request.TripRequestId , request.CancellationReason , passengerId))
                                 .ConfigureAwait(false);
        
        return HandleResult<bool, bool>(res);
    }
    
}