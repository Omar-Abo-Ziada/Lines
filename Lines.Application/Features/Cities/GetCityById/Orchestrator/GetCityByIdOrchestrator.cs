using Lines.Application.Common;
using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.Qureies;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Cities.Orchestrator;

public record GetCityByIdOrchestrator(Guid Id) : IRequest<Result<CityByIdDto>>;
public class GetCityByIdOrchestratorHandler : RequestHandlerBase<GetCityByIdOrchestrator, Result<CityByIdDto>>
{
    public GetCityByIdOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<CityByIdDto>> Handle(GetCityByIdOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCityByIdQuery(request.Id), cancellationToken).ConfigureAwait(false);
        return Result<CityByIdDto>.Success(result);
    }
}