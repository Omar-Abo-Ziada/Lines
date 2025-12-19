using Lines.Application.Features.Rewards.UpdateReward.Commands;

namespace Lines.Application.Features.Rewards.UpdateReward.Orchestrators
{
    public record UpdateRewardOrchestrator(Guid Id, string Title , string? Description , int PointsRequired ,
        decimal DiscountPercentage, decimal MaxValue) : IRequest<bool>;

    public class UpdateRewardOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<UpdateRewardOrchestrator, bool>(parameters)
    {
        public override async Task<bool> Handle(UpdateRewardOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateRewardCommand(request.Id, request.Title, request.Description, request.PointsRequired,
                                                         request.DiscountPercentage, request.MaxValue));
            return true;
        }
    }
}
