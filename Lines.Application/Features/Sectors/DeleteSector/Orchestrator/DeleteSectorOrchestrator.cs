using Lines.Application.Common;
using Lines.Application.Features.Cities;
using Lines.Application.Features.Common.Queries;
using Lines.Application.Features.Sectors.Command;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Sectors.Orchestrator;

public record DeleteSectorOrchestrator(Guid Id) : IRequest<Result<bool>>;
public class DeleteSectorOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<DeleteSectorOrchestrator, Result<bool>>(parameters)
{
    public override async Task<Result<bool>> Handle(DeleteSectorOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSectorCommand(request.Id), cancellationToken).ConfigureAwait(false);
        return Result<bool>.Success(result);
    }
}