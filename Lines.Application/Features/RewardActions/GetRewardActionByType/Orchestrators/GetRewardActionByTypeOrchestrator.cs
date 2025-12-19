using Lines.Application.Features.RewardActions.GetRewardActionByType.Queries;
using Lines.Application.Features.RewardActions.Shared.DTOs;

namespace Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators
{
    public record GetRewardActionByTypeOrchestrator(RewardActionType Type)
        : IRequest<Result<GetRewardActionsDTO>>;

    public class GetRewardActionByTypeOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetRewardActionByTypeOrchestrator, Result<GetRewardActionsDTO>>(parameters)
    {
        public override async Task<Result<GetRewardActionsDTO>> Handle(GetRewardActionByTypeOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetRewardActionByTypeQuery(request.Type), cancellationToken);

            return result == null
                ? Result<GetRewardActionsDTO>.Failure(new Error("404", "No reward actions found for the provided type.", ErrorType.NotFound))
                : Result<GetRewardActionsDTO>.Success(result);
        }
    }
}
