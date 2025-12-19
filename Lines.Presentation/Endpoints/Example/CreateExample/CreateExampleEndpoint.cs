using Lines.Application.Features.Examples;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Example.CreateExample;

public class CreateExampleEndpoint : BaseController<CreateExampleRequest, CreateExampleResponse>
{
    private readonly BaseControllerParams<CreateExampleRequest> _dependencyCollection;
    public CreateExampleEndpoint(BaseControllerParams<CreateExampleRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection ?? throw new ArgumentNullException(nameof(dependencyCollection));
    }
    
    [HttpPost("example")]
    public async Task<ApiResponse<CreateExampleResponse>> CreateExampleAsync([FromBody] CreateExampleRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
            return validationResult;
        
        var result = await _mediator.Send(new CreateExampleOrchestrator(request.Name,request.Description)).ConfigureAwait(false);

        return HandleResult<CreateExampleResponse,CreateExampleResponse>(result);   
    }
}