using Lines.Application.Common;
using Lines.Application.Features.Users.UpdatePassword.Commands;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Users.UpdatePassword.Orchestrators
{
    public record UpdatePasswordOrchestrator(Guid UserId, string NewPassword, string? CurrentPassword) : IRequest<Result>;

    public class UpdatePasswordOrchestratorHandler(
        RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<UpdatePasswordOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(UpdatePasswordOrchestrator request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new UpdatePasswordCommand(request.UserId, request.NewPassword, request.CurrentPassword),
                cancellationToken);

        }
    }
}
