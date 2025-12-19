using Lines.Application.Features.RewardActions.Shared.DTOs;
using Lines.Application.Features.Rewards.GetRewardActions.Queries;

namespace Lines.Application.Features.Rewards.GetRewardActions.Orchestrators
{
    public record GetAllRewardActionsOrchestrator(int pageNumber = 1, int pageSize = 10) : IRequest<PagingDto<GetRewardActionsDTO>>;

    public class GetRewardActionsOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetAllRewardActionsOrchestrator, PagingDto<GetRewardActionsDTO>>(parameters)
    {
        public override async Task<PagingDto<GetRewardActionsDTO>> Handle(GetAllRewardActionsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllRewardActionsQuery(request.pageNumber , request.pageSize));
            return result;
        }
    }
}
