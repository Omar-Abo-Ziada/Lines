using Lines.Application.Features.Rewards.GetAllRewards.Queries;
using Lines.Application.Features.Rewards.Shared.DTOs;

namespace Lines.Application.Features.Rewards.GetAllRewards.Orchestrators
{
    public record GetAllRewardsOrchestrator(int PageNumber = 1, int PageSize = 10) : IRequest<PagingDto<GetRewardDTO>>;


    public class GetAllRewardsOrchestratorHandler(RequestHandlerBaseParameters parameters) 
                    : RequestHandlerBase<GetAllRewardsOrchestrator, PagingDto<GetRewardDTO>>(parameters)
    {
        public override async Task<PagingDto<GetRewardDTO>> Handle(GetAllRewardsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllRewardsQuery(request.PageNumber, request.PageSize), cancellationToken);
            return result;
        }


    }   



}
