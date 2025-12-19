using Lines.Application.Common;
using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.Queries;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Cities.GetAllCities.Orchestrator;

public record GetAllCitiesOrchestrator(string? Name, int PageSize, int PageNumber) : IRequest<Result<PagingDto<GetAllCitiesDto>>>;
public class GetAllCitiesOrchestratorHandler : RequestHandlerBase<GetAllCitiesOrchestrator , Result<PagingDto<GetAllCitiesDto>>>
{
    public GetAllCitiesOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
    {
    }

    public override async Task<Result<PagingDto<GetAllCitiesDto>>> Handle(GetAllCitiesOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCitiesQuery(request.Name, request.PageSize, request.PageNumber), cancellationToken).ConfigureAwait(false);
        return Result<PagingDto<GetAllCitiesDto>>.Success(result);
    }
}