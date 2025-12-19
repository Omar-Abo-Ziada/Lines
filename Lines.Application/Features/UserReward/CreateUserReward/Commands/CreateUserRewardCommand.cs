namespace Lines.Application.Features.UserReward.CreateUserReward.Commands
{
    public record CreateUserRewardCommand(Guid UserId , Guid RewardId) : IRequest<Guid>;

    public class CreateUserRewardCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.User.UserReward> _repository) :
        RequestHandlerBase<CreateUserRewardCommand, Guid>(parameters)
    {
        public override async Task<Guid> Handle(CreateUserRewardCommand request, CancellationToken cancellationToken)
        {
            var userReward = new Domain.Models.User.UserReward(request.UserId, request.RewardId);

            await _repository.AddAsync(userReward, cancellationToken);

            return userReward.Id;
        }
    }
}
