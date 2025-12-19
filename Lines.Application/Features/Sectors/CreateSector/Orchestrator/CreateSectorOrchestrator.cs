using Lines.Application.Common;
using Lines.Application.Features.Cities;
using Lines.Application.Features.Common.Queries;
using Lines.Application.Features.Sectors.Command;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Sectors.Orchestrator;

public record CreateSectorOrchestrator(string Name, Guid CityId) : IRequest<Result<CreateSectorDto>>;
public class CreateSectorOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<CreateSectorOrchestrator, Result<CreateSectorDto>>(parameters)
{
    public override async Task<Result<CreateSectorDto>> Handle(CreateSectorOrchestrator request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new CreateSectorCommand(request.Name, request.CityId), cancellationToken).ConfigureAwait(false);
        return Result<CreateSectorDto>.Success(result);
    }

    private async Task<Result<CreateSectorDto>> ValidateRequest(CreateSectorOrchestrator request)
    {
        var isCityExsits = await _mediator.Send(new CheckIfCityExistQuery(request.CityId)).ConfigureAwait(false);
        return !isCityExsits ? Result<CreateSectorDto>.Failure(CityErrors.CityNotExistError) : Result<CreateSectorDto>.Success(null!);
    }
}