using Lines.Application.Features.RewardActions.GetRewardActionById.Orchestrators;
using Lines.Application.Features.RewardActions.UpdateRewardActions.Commands;
using Lines.Domain.Models.User;

namespace Lines.Application.Features.Rewards.UpdateRewardActions.Orchestrators
{
    public record UpdateRewardActionsOrchestrator(Guid Id , int NewPoints, string Name)
        : IRequest<bool>;

    public class UpdateRewardActionsOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<UpdateRewardActionsOrchestrator, bool>(parameters)
    {
        public override async Task<bool> Handle(UpdateRewardActionsOrchestrator request, CancellationToken cancellationToken)
        {
          

            var result = await _mediator.Send(new UpdateRewardActionsCommand(request.Id , request.NewPoints , request.Name), cancellationToken);
            return result;
        }
    }
}
