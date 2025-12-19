using Lines.Application.Common;
using Lines.Application.Features.Cities.UpdateCity;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Cities.Orchestrator;

public record UpdateCityOrchestrator(Guid Id, string Name) : IRequest<Result<bool>>;
public class UpdateCityOrchestratorHandler : RequestHandlerBase<UpdateCityOrchestrator, Result<bool>>
{
    public UpdateCityOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<bool>> Handle(UpdateCityOrchestrator request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateCityCommand(request.Id, request.Name), cancellationToken).ConfigureAwait(false);
        return Result<bool>.Success(true);
    }
}