using Lines.Application.Features.Trips.GetAllTripsByPassengerId.DTOs;
using Lines.Application.Features.Trips.GetAllTripsByPassengerId.Orchestrators;

namespace Lines.Presentation.Endpoints.Trips.GetAllTripsByPassengerId
{
    public class GetAllTripsByPassengerIdEndpoint
        : BaseController<GetAllTripsByPassengerIdRequest, List<GetAllTripsByPassengerIdDto>>
    {
        private readonly BaseControllerParams<GetAllTripsByPassengerIdRequest> _dependencyCollection;

        public GetAllTripsByPassengerIdEndpoint(BaseControllerParams<GetAllTripsByPassengerIdRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpGet("Trip/GetAllByPassengerId")]
        public async Task<ApiResponse<List<GetAllTripsByPassengerIdDto>>> HandleAsync(
            [FromQuery] GetAllTripsByPassengerIdRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var passengerId = GetCurrentPassengerId();
            var userId = GetCurrentUserId();

            var result = await _mediator.Send(
                new GetAllTripsByPassengerIdOrchestrator(passengerId , userId),
            cancellationToken).ConfigureAwait(false);

            return HandleResult<List<GetAllTripsByPassengerIdDto>, List<GetAllTripsByPassengerIdDto>>(result);
        }
    }
}
