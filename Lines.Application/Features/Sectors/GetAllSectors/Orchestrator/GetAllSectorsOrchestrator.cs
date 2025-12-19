using Lines.Application.Common;
using Lines.Application.Features.Cities;
using Lines.Application.Features.Common.Queries;
using Lines.Application.Features.Sectors.Command;
using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Sectors.Orchestrator;

public record GetAllSectorsOrchestrator(string? Name, Guid? CityId, int PageSize, int PageNumber) : IRequest<Result<PagingDto<GetAllSectorsDto>>>;
public class GetAllSectorsOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetAllSectorsOrchestrator, Result<PagingDto<GetAllSectorsDto>>>(parameters)
{
    public override async Task<Result<PagingDto<GetAllSectorsDto>>> Handle(GetAllSectorsOrchestrator request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSectorsQuery(request.Name, request.CityId, request.PageSize, request.PageNumber), cancellationToken).ConfigureAwait(false);
        return Result<PagingDto<GetAllSectorsDto>>.Success(result);
    }
}