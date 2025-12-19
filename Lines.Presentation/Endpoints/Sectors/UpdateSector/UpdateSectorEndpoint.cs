using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Features.Sectors.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Sectors;

public class UpdateSectorEndpoint : BaseController<UpdateSectorRequest, bool>
{
    private readonly BaseControllerParams<UpdateSectorRequest> _dependencyCollection;
    public UpdateSectorEndpoint(BaseControllerParams<UpdateSectorRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("sector/update")]
    public async Task<ApiResponse<bool>> Create([FromBody] UpdateSectorRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new UpdateSectorOrchestrator(request.Id, request.Name, request.CityId), cancellationToken).ConfigureAwait(false);
        return HandleResult<bool, bool>(result);
    }
}