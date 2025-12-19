using Lines.Application.Common;
using Lines.Application.Features.Cities;
using Lines.Application.Features.Common.Queries;
using Lines.Application.Features.Sectors.Command;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Sectors.Orchestrator;

public record GetSectorByIdOrchestrator(Guid Id) : IRequest<Result<GetSectorByIdDto>>;
public class GetSectorByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetSectorByIdOrchestrator, Result<GetSectorByIdDto>>(parameters)
{
    public override async Task<Result<GetSectorByIdDto>> Handle(GetSectorByIdOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSectorByIdQuery(request.Id), cancellationToken).ConfigureAwait(false);
        return Result<GetSectorByIdDto>.Success(result);
    }
}