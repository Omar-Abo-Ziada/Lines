using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Features.Sectors.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Sectors;

public class CreateSectorEndpoint : BaseController<CreateSectorRequest,CreateSectorResponse>
{
    private readonly BaseControllerParams<CreateSectorRequest> _dependencyCollection;
    public CreateSectorEndpoint(BaseControllerParams<CreateSectorRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("sector/create")]
    public async Task<ApiResponse<CreateSectorResponse>> Create([FromBody] CreateSectorRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new CreateSectorOrchestrator(request.Name, request.CityId), cancellationToken).ConfigureAwait(false);
        return HandleResult<CreateSectorDto, CreateSectorResponse>(result);
    }
}