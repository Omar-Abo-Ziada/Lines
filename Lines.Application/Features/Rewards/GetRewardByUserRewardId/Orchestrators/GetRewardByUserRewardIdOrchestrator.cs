using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Rewards.GetRewardByUserRewardId.Queries;
using Lines.Application.Features.UserReward.GetUnusedRewardByUserId.Orchestrators;
using Lines.Application.Features.UserReward.GetUnusedRewardByUserId.Queries;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Dtos;
using Lines.Domain.Models.User;

namespace Lines.Application.Features.Rewards.GetRewardByUserRewardId.Orchestrators
{
    public record GetRewardByUserRewardIdOrchestrator(Guid? UserRewardID) : IRequest<Result<Reward>>;
    public class GetRewardByUserRewardIdOrchestratorHandler(RequestHandlerBaseParameters parameters) :
      RequestHandlerBase<GetRewardByUserRewardIdOrchestrator, Result<Reward>>(parameters)
    {
        public override async Task<Result<Reward>> Handle(GetRewardByUserRewardIdOrchestrator request, CancellationToken cancellationToken)
        {
            var reward = await _mediator.Send(new GetRewardByUserRewardIdQuery(request.UserRewardID.Value));
            return reward != null ? Result<Reward>.Success(reward)
                                        : Result<Reward>.Failure(Error.NullValue);
        }
    }
}
