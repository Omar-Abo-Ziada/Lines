using Lines.Application.Common;
using Lines.Application.Features.Activities.DeleteActivitiesByUserId.Commands;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Activities.DeleteActivitiesByUserId.Orchestrator
{
    public record DeleteActivitiesByUserIdOrchestrator(Guid UserId) : IRequest<Result<bool>>;

    public class DeleteActivitiesByUserIdOrchestratorHandler(
            RequestHandlerBaseParameters parameters)
            : RequestHandlerBase<DeleteActivitiesByUserIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteActivitiesByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            var isDeleted = await _mediator.Send(new DeleteActivitiesByUserIdCommand(request.UserId), cancellationToken)
                                           .ConfigureAwait(false);

            if (!isDeleted)
                return Result<bool>.Failure(Error.General);

            return Result<bool>.Success(true);
        }
    }

}
