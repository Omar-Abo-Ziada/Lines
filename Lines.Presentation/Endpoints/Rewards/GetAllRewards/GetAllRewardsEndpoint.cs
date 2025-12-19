using Lines.Application.Extensions;
using Lines.Application.Features.Rewards.GetAllRewards.Orchestrators;
using Lines.Application.Features.Rewards.Shared.DTOs;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Rewards.GetAllRewards
{
    public class GetAllRewardsEndpoint
        : BaseController<GetAllRewardsRequest, PagingDto<GetAllRewardsResponse>>
    {
        private readonly BaseControllerParams<GetAllRewardsRequest> _dependencyCollection;

        public GetAllRewardsEndpoint(BaseControllerParams<GetAllRewardsRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpGet("rewards")]
        public async Task<ApiResponse<PagingDto<GetAllRewardsResponse>>> HandleAsync(
            [FromQuery] GetAllRewardsRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(
                new GetAllRewardsOrchestrator(request.PageNumber, request.PageSize),
                cancellationToken
            ).ConfigureAwait(false);

            return ApiResponse<PagingDto<GetAllRewardsResponse>>
                .SuccessResponse(result.AdaptPaging<GetRewardDTO, GetAllRewardsResponse>(), 200);
        }
    }
}
