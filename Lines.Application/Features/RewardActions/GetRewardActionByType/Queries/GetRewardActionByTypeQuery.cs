using Lines.Application.Features.RewardActions.Shared.DTOs;
using Lines.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.RewardActions.GetRewardActionByType.Queries
{
    public record GetRewardActionByTypeQuery(RewardActionType type) : IRequest<GetRewardActionsDTO>;


    public class GetRewardActionByTypeQueryHandler(RequestHandlerBaseParameters parameters, IRepository<RewardAction> _repository)
        : RequestHandlerBase<GetRewardActionByTypeQuery, GetRewardActionsDTO>(parameters)
    {
        public override async Task<GetRewardActionsDTO> Handle(GetRewardActionByTypeQuery request, CancellationToken cancellationToken)
        {
            var rewardAction =  await _repository.Get(r => r.Type == request.type).FirstOrDefaultAsync();

            if (rewardAction == null)
            {
                return null;
            }

            var dto = rewardAction.Adapt<GetRewardActionsDTO>();
            return dto;
        }
    }
}
