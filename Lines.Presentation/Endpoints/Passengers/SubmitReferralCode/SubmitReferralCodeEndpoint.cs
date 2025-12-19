using Lines.Application.Features.Passengers.SubmitReferralCode.Orchestrators;

namespace Lines.Presentation.Endpoints.Passengers.SubmitReferralCode
{
    public class SubmitReferralCodeEndpoint
        : BaseController<SubmitReferralCodeRequest, bool>
    {
        public SubmitReferralCodeEndpoint(
            BaseControllerParams<SubmitReferralCodeRequest> dependencyCollection)
            : base(dependencyCollection)
        {
        }

        [HttpPost("passengers/submitreferralcode")]
        public async Task<ApiResponse<bool>> HandleAsync(
            [FromBody] SubmitReferralCodeRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var userId = GetCurrentUserId();

            var result = await _mediator.Send(
                new SubmitReferralCodeOrchestrator(userId, request.ReferralCode),
                cancellationToken);

            return result.IsSuccess
                ? ApiResponse<bool>.SuccessResponse(true)
                : ApiResponse<bool>.ErrorResponse(result.Error, 400);
        }
    }
}