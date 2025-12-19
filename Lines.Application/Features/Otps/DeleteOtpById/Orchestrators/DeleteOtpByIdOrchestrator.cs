using Lines.Application.Common;
using Lines.Application.Features.Otps.DeleteOtpById.Commands;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Otps.DeleteOtpById.Orchestrators
{
    public record DeleteOtpByIdOrchestrator (Guid Id) : IRequest<Result<Unit>>;

    public class DeleteOtpByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<DeleteOtpByIdOrchestrator, Result<Unit>>(parameters)
    {
        public override async Task<Result<Unit>> Handle(DeleteOtpByIdOrchestrator request, CancellationToken cancellationToken)
        {
            // Delegate to the DeleteOtpByIdCommand
            var result = await _mediator.Send(new DeleteOtpByIdCommand(request.Id), cancellationToken);

            return result == true
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure(Error.NullValue);
        }
    }
}
