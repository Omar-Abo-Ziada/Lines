using Lines.Application.Features.Rewards.GetRewardById.Queries;

namespace Lines.Application.Features.Rewards.GetRewardById.Orchestrators
{
    public record GetRewardByIdOrchestrator(Guid RewardId) : IRequest<Result<Lines.Domain.Models.User.Reward>>;

    public class GetRewardByIdOrchestratorHandler(RequestHandlerBaseParameters parameters) :
        RequestHandlerBase<GetRewardByIdOrchestrator, Result<Domain.Models.User.Reward>>(parameters)
    {
        public override async Task<Result<Domain.Models.User.Reward>> Handle(GetRewardByIdOrchestrator request, CancellationToken cancellationToken)
        {
            var reward = await _mediator.Send(new GetRewardByIdQuery(request.RewardId), cancellationToken);

            return reward != null ? Result<Domain.Models.User.Reward>.Success(reward)
                                        : Result<Domain.Models.User.Reward>.Failure(Error.NullValue);
        }
    }
}
