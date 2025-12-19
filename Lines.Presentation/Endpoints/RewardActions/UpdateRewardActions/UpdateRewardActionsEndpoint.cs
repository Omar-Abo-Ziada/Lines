using Lines.Application.Features.Rewards.UpdateRewardActions.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.RewardActions.UpdateRewardActions
{
    public class UpdateRewardActionsEndpoint
        : BaseController<UpdateRewardActionsRequest, bool>
    {
        private readonly BaseControllerParams<UpdateRewardActionsRequest> _dependencyCollection;

        public UpdateRewardActionsEndpoint(BaseControllerParams<UpdateRewardActionsRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPut("rewards/actions/update")]
        public async Task<ApiResponse<bool>> HandleAsync(
            [FromBody] UpdateRewardActionsRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(
                new UpdateRewardActionsOrchestrator(request.Id, request.NewPoints, request.Name),
                cancellationToken).ConfigureAwait(false);

            return result == true ? ApiResponse<bool>.SuccessResponse(result, 200) :
                                   ApiResponse<bool>.ErrorResponse(Error.General, 400);
        }
    }
}
