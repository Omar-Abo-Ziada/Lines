using Lines.Application.Extensions;
using Lines.Application.Features.RewardActions.Shared.DTOs;
using Lines.Application.Features.Rewards.GetRewardActions.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.RewardActions.GetRewardActions
{
    public class GetRewardActionsEndpoint : BaseController<GetRewardActionsRequest, PagingDto<GetRewardActionsResponse>>
    {
        private readonly BaseControllerParams<GetRewardActionsRequest> _dependencyCollection;

        public GetRewardActionsEndpoint(BaseControllerParams<GetRewardActionsRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpGet("rewards/actions")]
        public async Task<ApiResponse<PagingDto<GetRewardActionsResponse>>> HandleAsync([FromQuery] GetRewardActionsRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(new GetAllRewardActionsOrchestrator(request.PageNumber, request.PageSize),
                cancellationToken)
                                        .ConfigureAwait(false);

            return ApiResponse<PagingDto<GetRewardActionsResponse>>
                .SuccessResponse(result.AdaptPaging<GetRewardActionsDTO, GetRewardActionsResponse>(), 200);
        }
    }
}
