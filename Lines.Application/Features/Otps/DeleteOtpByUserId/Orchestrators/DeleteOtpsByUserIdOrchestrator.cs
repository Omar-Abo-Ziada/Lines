using Lines.Application.Common;
using Lines.Application.Features.Otps.DeleteOtpById.Orchestrators;
using Lines.Application.Features.Otps.GetOtpByUserId.Orchestrators;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Otps.DeleteOtpByUserId.Orchestrators
{
    public record DeleteOtpsByUserIdOrchestrator(Guid UserId) : IRequest<Result<Unit>>;
    public class DeleteOtpOrchestratorHandler(RequestHandlerBaseParameters parameters)
       : RequestHandlerBase<DeleteOtpsByUserIdOrchestrator, Result<Unit>>(parameters)
    {
        public override async Task<Result<Unit>> Handle(DeleteOtpsByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            // Delegate to the DeleteOtpByUserIdCommand
            var result = await _mediator.Send(new GetOtpByUserIdOrchestrator(request.UserId), cancellationToken);

            if(result.IsFailure)
            {
                return Result<Unit>.Failure(result.Error);
            }

            
            if(result.Value is not null)
            {
                var deletionResult = await _mediator.Send(new DeleteOtpByIdOrchestrator(result.Value.Id), cancellationToken);
            if (deletionResult.IsFailure)
                {
                    return Result<Unit>.Failure(deletionResult.Error);
                }
            }
            
            

            return Result<Unit>.Success(Unit.Value);
               
        }
    }
}
