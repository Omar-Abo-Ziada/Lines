using Lines.Application.Features.Passengers.IsReferralCodeSubmittedByPassenger.Orchestrators;

namespace Lines.Presentation.Endpoints.Passengers.IsReferralCodeSubmittedByPassenger
{
    public class IsReferralCodeSubmittedByPassengerEndpoint
        : BaseController<IsReferralCodeSubmittedByPassengerRequest, bool>
    {
        public IsReferralCodeSubmittedByPassengerEndpoint(
            BaseControllerParams<IsReferralCodeSubmittedByPassengerRequest> dependencyCollection)
            : base(dependencyCollection)
        {
        }

        [HttpGet("passengers/isreferralcodesubmitted")]
        public async Task<ApiResponse<bool>> HandleAsync(
            [FromQuery] IsReferralCodeSubmittedByPassengerRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(
                new IsReferralCodeSubmittedByPassengerOrchestrator(userId),
                cancellationToken);

            return result.IsSuccess ? ApiResponse<bool>.SuccessResponse(result.Value, 200)
                                    : ApiResponse<bool>.ErrorResponse(result.Error, 400); 
        }
    }
}