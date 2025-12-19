using Lines.Application.Common;
using Lines.Application.Features.Cities;
using Lines.Application.Features.Common.Queries;
using Lines.Application.Features.Sectors.Command;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Sectors.Orchestrator;

public record UpdateSectorOrchestrator(Guid Id, string Name, Guid CityId) : IRequest<Result<bool>>;
public class UpdateSectorOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<UpdateSectorOrchestrator, Result<bool>>(parameters)
{
    public override async Task<Result<bool>> Handle(UpdateSectorOrchestrator request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new UpdateSectorCommand(request.Id, request.Name, request.CityId), cancellationToken).ConfigureAwait(false);
        return Result<bool>.Success(result);
    }

    private async Task<Result<bool>> ValidateRequest(UpdateSectorOrchestrator request)
    {
        var isCityExsits = await _mediator.Send(new CheckIfCityExistQuery(request.CityId)).ConfigureAwait(false);
        return !isCityExsits ? Result<bool>.Failure(CityErrors.CityNotExistError) : Result<bool>.Success(true);
    }
}