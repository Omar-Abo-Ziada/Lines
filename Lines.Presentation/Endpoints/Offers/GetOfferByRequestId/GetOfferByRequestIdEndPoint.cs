using Lines.Application.Features.Notifications.GetNotifications.Orchestrator;
using Lines.Application.Features.Notifications.ReadNotifications.Orchestrator;
using Lines.Application.Features.Offers.GetOfferByRequestId.Orchestrator;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Offers.GetOfferByRequestId;

[AllowAnonymous]
public class GetOfferByRequestIdEndPoint : BaseController<GetOfferByRequestIdRequest, GetOfferByRequestIdResponse>
{
    private readonly BaseControllerParams<GetOfferByRequestIdRequest> _dependencyCollection;

    public GetOfferByRequestIdEndPoint(BaseControllerParams<GetOfferByRequestIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;

    }
    [HttpGet("GetOfferByRequestId")]
    public async Task<ApiResponse<GetOfferByRequestIdResponse>> ReadNotifications([FromQuery] GetOfferByRequestIdRequest request)
    {
        var res = await _mediator.Send(new GetOfferByRequestIdOrchestrator(request.RequestId)).ConfigureAwait(false);

        if (res.IsSuccess)
        {
            var response = new GetOfferByRequestIdResponse
            {
                Offers = res.Value,
            };
            return ApiResponse<GetOfferByRequestIdResponse>.SuccessResponse(response);
        }

        return ApiResponse<GetOfferByRequestIdResponse>.ErrorResponse(res.Error, 400);
    }
}

