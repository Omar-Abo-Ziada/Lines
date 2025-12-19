using Lines.Application.Features.TermsAndConditions.DeleteTerms.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TermsAndConditions.DeleteTerms;

public class DeleteTermsEndPoint : BaseController<DeleteTermsRequest, bool>
{
    private BaseControllerParams<DeleteTermsRequest> _dependencyCollection;
    public DeleteTermsEndPoint(BaseControllerParams<DeleteTermsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpDelete("TermsAndConditions/Delete")]
    public async Task<ApiResponse<bool>> HandleAsync(DeleteTermsRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new DeleteTermsOrchestrator(request.Id), cancellationToken);

        return HandleResult<bool, bool>(result);



    }
}
