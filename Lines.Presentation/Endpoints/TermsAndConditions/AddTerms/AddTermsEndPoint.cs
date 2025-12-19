using Lines.Application.Features.TermsAndConditions.AddTerms.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TermsAndConditions.AddTerms;

public class AddTermsEndPoint : BaseController<AddTermsRequest, Guid>
{
    private BaseControllerParams<AddTermsRequest> _dependencyCollection;
    public AddTermsEndPoint(BaseControllerParams<AddTermsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpPost("TermsAndConditions/Add")]
    public async Task<ApiResponse<Guid>> HandleAsync(AddTermsRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new AddTermsOrchestrator(request.Type, request.Header, request.Content, request.Order), cancellationToken);

        return result.IsSuccess ?
            ApiResponse<Guid>.SuccessResponse(result.Value, 200) :
            ApiResponse<Guid>.ErrorResponse(result.Error, 400);



    }
}
