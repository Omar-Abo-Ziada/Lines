namespace Lines.Application.Features.Rewards.UpdateReward.Commands
{
    public record UpdateRewardCommand(Guid Id, string Title, string? Description, int PointsRequired,
        decimal DiscountPercentage, decimal MaxValue) : IRequest<bool>;


    public class UpdateRewardCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.User.Reward> repository)
        : RequestHandlerBase<UpdateRewardCommand, bool>(parameters)
    {
        public override async Task<bool> Handle(UpdateRewardCommand request, CancellationToken cancellationToken)
        {
            var reward = await repository.GetByIdAsync(request.Id);

            reward.Title = request.Title;
            reward.Description = request.Description;
            reward.PointsRequired = request.PointsRequired;
            reward.DiscountPercentage = request.DiscountPercentage;
            reward.MaxValue = request.MaxValue;

            await repository.UpdateAsync(reward, cancellationToken);
            return true;
        }
    }
}
