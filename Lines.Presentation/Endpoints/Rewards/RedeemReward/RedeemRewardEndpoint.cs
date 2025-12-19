using Lines.Application.Features.Rewards.RedeemReward.Orcestrators;

namespace Lines.Presentation.Endpoints.Rewards.RedeemReward
{
    public class RedeemRewardEndpoint : BaseController<RedeemRewardRequest, bool>
    {
        private readonly BaseControllerParams<RedeemRewardRequest> _dependencyCollection;

        public RedeemRewardEndpoint(BaseControllerParams<RedeemRewardRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("rewards/redeem")]
        public async Task<ApiResponse<bool>> HandleAsync([FromBody] RedeemRewardRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
            var userId = GetCurrentUserId();
            var passengerId = GetCurrentPassengerId();

            var result = await _mediator.Send(new RedeemRewardOrchestrator(request.RewardId,userId, passengerId), cancellationToken)
                                        .ConfigureAwait(false);

            return HandleResult<bool , bool>(result);
        }
    }
}
