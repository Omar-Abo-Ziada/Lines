using Lines.Application.Features.UserReward.CreateUserReward.Commands;

namespace Lines.Application.Features.UserReward.CreateUserReward.Orchestrators
{
    public record CreateUserRewardOrchestrator(Guid UserId , Guid RewardId) : IRequest<Result>;


    public class CreateUserRewardOrchestratorHandler(RequestHandlerBaseParameters parameters) :
        RequestHandlerBase<CreateUserRewardOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(CreateUserRewardOrchestrator request, CancellationToken cancellationToken)
        {
            var userRewardId = await _mediator.Send(new CreateUserRewardCommand(request.UserId, request.RewardId));
            return userRewardId == Guid.Empty? Result.Failure(Error.General) : Result.Success();
        }
    }
}
