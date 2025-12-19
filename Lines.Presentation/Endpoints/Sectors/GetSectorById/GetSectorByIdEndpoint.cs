using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Features.Sectors.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Sectors;

public class GetSectorByIdEndpoint : BaseController<GetSectorByIdRequest,GetSectorByIdResponse>
{
    private readonly BaseControllerParams<GetSectorByIdRequest> _dependencyCollection;
    public GetSectorByIdEndpoint(BaseControllerParams<GetSectorByIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("sector/getbyid")]
    public async Task<ApiResponse<GetSectorByIdResponse>> Create([FromQuery] GetSectorByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSectorByIdOrchestrator(request.Id), cancellationToken).ConfigureAwait(false);
        return HandleResult<GetSectorByIdDto, GetSectorByIdResponse>(result);
    }
}