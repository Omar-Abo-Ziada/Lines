using Lines.Application.Features.TermsAndConditions.EditTerms.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TermsAndConditions.EditTerms;

public class EditTermsEndPoint : BaseController<EditTermsRequest, bool>
{
    private BaseControllerParams<EditTermsRequest> _dependencyCollection;
    public EditTermsEndPoint(BaseControllerParams<EditTermsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpPut("TermsAndConditions/Edit")]
    public async Task<ApiResponse<bool>> HandleAsync(EditTermsRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new EditTermsOrchestrator(request.Id, request.Header, request.Content, request.Order), cancellationToken);

        return HandleResult<bool, bool>(result);



    }
}
