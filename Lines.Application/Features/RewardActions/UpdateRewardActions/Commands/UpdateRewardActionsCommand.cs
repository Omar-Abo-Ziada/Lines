using Lines.Domain.Models.User;

namespace Lines.Application.Features.RewardActions.UpdateRewardActions.Commands
{
    public record UpdateRewardActionsCommand(Guid Id, int NewPoints, string Name) : IRequest<bool>;


    public class UpdateRewardActionsCommandHandler(RequestHandlerBaseParameters parameters, IRepository<RewardAction> repository)
        : RequestHandlerBase<UpdateRewardActionsCommand, bool>(parameters)
    {
        public override async Task<bool> Handle(UpdateRewardActionsCommand request, CancellationToken cancellationToken)
        {
            var rewardAction = await repository.GetByIdAsync(request.Id);

            rewardAction.Points = request.NewPoints;
            rewardAction.Name = request.Name;

            await repository.UpdateAsync(rewardAction, cancellationToken);
            return true;
        }
    }
}
