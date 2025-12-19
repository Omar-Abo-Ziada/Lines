using Lines.Application.Features.Feedbacks.DeleteFeedbackByUserId.Commands;

namespace Lines.Application.Features.Feedback.DeleteFeedbackByUserId.Orchestrator
{
    public record DeleteFeedbackByUserIdOrchestrator(Guid UserId) : IRequest<Result<bool>>;

    public class DeleteFeedbackByUserIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteFeedbackByUserIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteFeedbackByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteFeedbackByUserIdCommand(request.UserId), cancellationToken)
                           .ConfigureAwait(false);

            return Result<bool>.Success(true);
        }
    }
} 