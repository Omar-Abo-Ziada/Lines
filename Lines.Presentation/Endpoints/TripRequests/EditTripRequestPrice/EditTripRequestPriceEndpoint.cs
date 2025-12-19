
using Lines.Application.Features.TripRequests.EditTripRequestPrice.Orchestrator;

namespace Lines.Presentation.Endpoints.TripRequests.EditTripRequestPrice
{
    [Route("api/trip-requests")]
    [ApiController]
    public class EditTripRequestPriceEndpoint : BaseController<EditTripRequestPriceRequest, EditTripRequestPriceResponse>
    {

        public EditTripRequestPriceEndpoint(BaseControllerParams<EditTripRequestPriceRequest> dependencyCollection):base(dependencyCollection)
        {
            
        }

        /// <summary>
        ///edit trip request price 
        /// </summary>
        [HttpPut("Editprice")]
        [Authorize]
        public async Task<IActionResult> EditTripRequestPriceAsync(
        [FromBody] EditTripRequestPriceRequest request,
        CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var result = await _mediator.Send(
                new EditTripRequestPriceOrchestrator(request.TripRequestId, request.NewPrice, userId),
                cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            var response = new EditTripRequestPriceResponse(request.TripRequestId, request.NewPrice);
            return Ok(response);
        }

        
    }


}



//using Lines.Application.Features.TripRequests.Orchestrator;
//using Lines.Application.Shared;
//using Lines.Domain.Shared;
//using Lines.Presentation.Common;
//using Lines.Presentation.Endpoints.Notifications.GetNotifications;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Lines.Presentation.Endpoints.TripRequests.EditTripRequestPrice;

//public class EditTripRequestPriceEndpoint : BaseController<EditTripRequestPriceRequest, bool>
//{
//    private BaseControllerParams<EditTripRequestPriceRequest> _dependencyCollection;
//    public EditTripRequestPriceEndpoint(BaseControllerParams<EditTripRequestPriceRequest> dependencyCollection) : base(dependencyCollection)
//    {
//        _dependencyCollection = dependencyCollection;
//    }

//    [HttpPost("trip-request/create")]
//    [Authorize]
//    public async Task<ApiResponse<bool>> Create([FromBody] EditTripRequestPriceRequest request)
//    {
//        var validateRequest = await ValidateRequestAsync(request);
//        if (!validateRequest.IsSuccess)
//        {
//            return validateRequest;
//        }
//        var userId = GetCurrentUserId();
//        if (userId == Guid.Empty)
//        {
//            return HandleResult<EditTripRequestPriceRequest, bool>(
//                Result<EditTripRequestPriceRequest>.Failure(new Error(Code: "UNAUTHORIZED", Description: "User not authenticated", Type: ErrorType.Validation)));
//        }

//        var res = await _mediator.Send(new EditTripRequestPriceOrchestrator(
//            userId, 
//            request.StartLocation,
//            request.EndLocations,
//            request.IsScheduled,
//            request.ScheduledAt,
//            request.VehicleTypeId,
//            request.PaymentMethodId,
//            request.EstimatedPrice,
//            request.Distance)).ConfigureAwait(false);

//        return HandleResult<bool, bool>(res);
//    }

//}