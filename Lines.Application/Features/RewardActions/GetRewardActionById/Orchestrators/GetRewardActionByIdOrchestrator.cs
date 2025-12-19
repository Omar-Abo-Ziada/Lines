using Lines.Application.Features.RewardActions.GetRewardActionById.Queries;
using Lines.Application.Features.RewardActions.Shared.DTOs;

namespace Lines.Application.Features.RewardActions.GetRewardActionById.Orchestrators
{
    public record GetRewardActionByIdOrchestrator(Guid Id) : IRequest<Result<GetRewardActionsDTO>>;


    public class GetRewardActionByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetRewardActionByIdOrchestrator, Result<GetRewardActionsDTO>>(parameters)
    {
        public override async Task<Result<GetRewardActionsDTO>> Handle(GetRewardActionByIdOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetRewardActionByIdQuery(request.Id), cancellationToken);
            return result == null ? Result<GetRewardActionsDTO>.Failure(new Error("404" , "No reward action found with the provided ID." , ErrorType.NotFound)) 
                                    : Result<GetRewardActionsDTO>.Success(result);
        }
    }

}
