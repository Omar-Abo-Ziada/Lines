using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.TermsAndConditions.GetTermsandConditions;

[AllowAnonymous]
public class GetTermsAndConditionsEndPoint : BaseController<GetTermsAndConditionsRequest, GetTermsAndConditionsResponse>
{
    private readonly BaseControllerParams<GetTermsAndConditionsRequest> _dependencyCollection;

    public GetTermsAndConditionsEndPoint(BaseControllerParams<GetTermsAndConditionsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;

    }
    [HttpGet("TermsAndConditions/GetAll")]
    public async Task<ApiResponse<GetTermsAndConditionsResponse>> GetTermsAndConditions()
    {
        var res = await _mediator.Send(new GetTermsAndConditionsOrchestrator()).ConfigureAwait(false);

        if (res.IsSuccess)
        {
            var response = new GetTermsAndConditionsResponse
            {
                TermsAndConditions = res.Value
            };
            return ApiResponse<GetTermsAndConditionsResponse>.SuccessResponse(response);
        }

        return ApiResponse<GetTermsAndConditionsResponse>.ErrorResponse(res.Error, 400);
    }
}

