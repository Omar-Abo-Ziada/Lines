using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Lines.Application.Features.Offers.CreateOffer.Orchestrators;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Offers.CreateOffer;

public class CreateOfferEndpoint : BaseController<CreateOfferRequest, CreateOfferReponse>
{
    private BaseControllerParams<CreateOfferRequest> _dependencyCollection;

    public CreateOfferEndpoint(BaseControllerParams<CreateOfferRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [Authorize]
    [HttpPost("trip-request/offer")]
    public async Task<ApiResponse<CreateOfferReponse>> CreateOffer([FromBody] CreateOfferRequest request)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }

        // Get the current user ID from the authenticated user
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<OfferDTO, CreateOfferReponse>(
                Result<OfferDTO>.Failure(new Error(Code: "UNAUTHORIZED", Description: "User not authenticated", Type: ErrorType.Validation)));
        }

        var result = await _mediator.Send(new CreateOfferOrchestrator(request.estimatedPrice, request.distanceToArriveInMeters, request.timeToArriveInMinutes, request.tripRequestId, userId));

        return HandleResult<OfferDTO, CreateOfferReponse>(result);
    }
}