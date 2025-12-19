using Lines.Application.Common;
using Lines.Application.Features.EmergencyNumbers.DeleteEmergencyNumbersByUserId.Commands;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.EmergencyNumbers.DeleteEmergencyNumbersByUserId.Orchestrator
{
    public record DeleteEmergencyNumbersByUserIdOrchestrator(Guid UserId) : IRequest<Result<bool>>;


    public class DeleteEmergencyNumbersByUserIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteEmergencyNumbersByUserIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteEmergencyNumbersByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteEmergencyNumbersByUserIdCommand(request.UserId), cancellationToken).ConfigureAwait(false);
            return Result<bool>.Success(true);
        }
    }
}
