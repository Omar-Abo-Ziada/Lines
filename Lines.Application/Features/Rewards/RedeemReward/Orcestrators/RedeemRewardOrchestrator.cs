using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Features.Rewards.GetRewardById.Orchestrators;
using Lines.Application.Features.UserReward.CreateUserReward.Orchestrators;

namespace Lines.Application.Features.Rewards.RedeemReward.Orcestrators
{
    public record RedeemRewardOrchestrator(Guid RewardId , Guid UserId , Guid PassengerId) : IRequest<Result<bool>>;

    public class RedeemRewardOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<RedeemRewardOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(RedeemRewardOrchestrator request, CancellationToken cancellationToken)
        {
            // check if user has enough points to redeem this reward
            var passenger = await _mediator.Send(new GetPassengerByIdOrchestrator(request.PassengerId));
            var reward = await _mediator.Send(new GetRewardByIdOrchestrator(request.RewardId));
            
            if(passenger.Value.Points < reward.Value.PointsRequired)
            {
                return Result<bool>.Failure(RewardErrors.MorePointsRequiredError());
            }

            // deduct points from passenger
            passenger.Value.DeductRewardPoints(reward.Value.PointsRequired);

            // insert record in user rewards 
            var result = await _mediator.Send(new CreateUserRewardOrchestrator(request.UserId , request.RewardId));
            return result.IsSuccess ? true : false;
        }
    }


}
