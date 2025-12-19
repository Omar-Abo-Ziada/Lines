using Lines.Application.Features.Rewards.UpdateReward.Orchestrators;

namespace Lines.Presentation.Endpoints.Rewards.UpdateReward
{
    public class UpdateRewardEndpoint : BaseController<UpdateRewardRequest, bool>
    {
        private readonly BaseControllerParams<UpdateRewardRequest> _dependencyCollection;

        public UpdateRewardEndpoint(BaseControllerParams<UpdateRewardRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPut("rewards/update")]
        public async Task<ApiResponse<bool>> HandleAsync([FromBody] UpdateRewardRequest request,CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(
                new UpdateRewardOrchestrator(request.Id , request.Title , request.Description , request.PointsRequired, 
                                             request.DiscountPercentage, request.MaxValue), cancellationToken)
                .ConfigureAwait(false);

            return ApiResponse<bool>.SuccessResponse(result, 200);
        }
    }
}
