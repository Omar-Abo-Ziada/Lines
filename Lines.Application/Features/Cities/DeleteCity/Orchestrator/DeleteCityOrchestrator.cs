using Lines.Application.Common;
using Lines.Application.Features.Cities.DeleteCity.Command;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Cities.DeleteCity.Orchestrator;

public record DeleteCityOrchestrator(Guid Id) : IRequest<Result<bool>>;
public class DeleteCityOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<DeleteCityOrchestrator, Result<bool>>(parameters)
{
    public override async Task<Result<bool>> Handle(DeleteCityOrchestrator request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCityCommand(request.Id), cancellationToken).ConfigureAwait(false);
        return Result<bool>.Success(true);
    }
}