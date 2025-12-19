//using Lines.Application.Features.Trips.PassengerStart.Orchestrators;
//using Lines.Application.Shared;
//using Lines.Presentation.Common;
//using Microsoft.AspNetCore.Mvc;

//namespace Lines.Presentation.Endpoints.Trips.PassengerRequestStart
//{
//    /**
//     * Summary:
//     * Allows the passenger to request starting the trip after the driver has arrived.
//     * Preconditions:
//     *  - Trip exists and belongs to the current passenger
//     *  - DriverArrivedAt is set
//     * Behavior:
//     *  - Sets PassengerStartRequestedAt = UtcNow (idempotent)
//     *  - Returns 200 with { requested: true } on success
//     */
//    [Route("api/trips")]
//    [ApiController]
//    public class PassengerRequestStartEndpoint : BaseController<PassengerRequestStartRequest, PassengerRequestStartResponse>
//    {
//        public PassengerRequestStartEndpoint(BaseControllerParams<PassengerRequestStartRequest> dependencyCollection)
//            : base(dependencyCollection) { }

//        [HttpPut("start-request")]
//        public async Task<ApiResponse<PassengerRequestStartResponse>> RequestStart(
//            [FromBody] PassengerRequestStartRequest request,
//            CancellationToken ct)
//        {
//            var validation = await ValidateRequestAsync(request);
//            if (!validation.IsSuccess)
//                return validation;

//            var passengerId = GetCurrentPassengerId();
//            if (passengerId == Guid.Empty)
//            {
//                var err = new Error("Auth.Unauthorized", "Passenger is not authenticated.", ErrorType.Validation);
//                return ApiResponse<PassengerRequestStartResponse>.ErrorResponse(err, 401);
//            }

//            var result = await _mediator.Send(
//                new PassengerRequestStartOrchestrator(request.TripId, passengerId),
//                ct);

//             if (result.IsSuccess)
//                return ApiResponse<PassengerRequestStartResponse>.SuccessResponse(new PassengerRequestStartResponse(true));

//            return ApiResponse<PassengerRequestStartResponse>.ErrorResponse(result.Error, 400);
//        }
//    }
//}

 
