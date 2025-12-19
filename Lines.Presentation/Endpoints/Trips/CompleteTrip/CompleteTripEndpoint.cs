using Lines.Application.Features.Trips.CompleteTrip.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Trips.CompleteTrip
{
    public class CompleteTripEndpoint : BaseController<CompleteTripRequest, bool>

    {
        private readonly BaseControllerParams<CompleteTripRequest> _dependencyCollection;

        public CompleteTripEndpoint(BaseControllerParams<CompleteTripRequest> dependencyCollection) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }


        [HttpPut("Trip/Complete")]
        public async Task<ApiResponse<bool>> HandleAsync([FromBody] CompleteTripRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var userId = GetCurrentUserId();

            var result = await _mediator.Send(new CompleteTripOrchestrator(request.tripId, userId)
                                                             , cancellationToken)
                                        .ConfigureAwait(false);

            return HandleResult<bool, bool>(result);

        }
    }
}
