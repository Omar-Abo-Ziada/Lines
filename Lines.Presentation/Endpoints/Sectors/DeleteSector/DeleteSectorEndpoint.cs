using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Features.Sectors.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Sectors;

public class DeleteSectorEndpoint : BaseController<DeleteSectorRequest, bool>
{
    private readonly BaseControllerParams<DeleteSectorRequest> _dependencyCollection;
    public DeleteSectorEndpoint(BaseControllerParams<DeleteSectorRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpDelete("sector/delete")]
    public async Task<ApiResponse<bool>> Create([FromBody] DeleteSectorRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new DeleteSectorOrchestrator(request.Id), cancellationToken).ConfigureAwait(false);
        return HandleResult<bool, bool>(result);
    }
}