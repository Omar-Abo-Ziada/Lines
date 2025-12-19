using Lines.Application.Features.Sectors.DTOs;
using Lines.Application.Features.Sectors.Orchestrator;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Sectors;

public class GetAllSectorsEndpoint : BaseController<GetAllSectorsRequest,PagingDto<GetAllSectorsResponse>>
{
    private readonly BaseControllerParams<GetAllSectorsRequest> _dependencyCollection;
    public GetAllSectorsEndpoint(BaseControllerParams<GetAllSectorsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("sector/getall")]
    public async Task<ApiResponse<PagingDto<GetAllSectorsResponse>>> Create([FromQuery] GetAllSectorsRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSectorsOrchestrator(request.Name, request.CityId, request.PageSize, request.PageNumber), cancellationToken).ConfigureAwait(false);
        return HandleResult<PagingDto<GetAllSectorsDto>, PagingDto<GetAllSectorsResponse>>(result);
    }
}