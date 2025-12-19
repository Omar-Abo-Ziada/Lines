using Lines.Application.Features.Offers.AcceptOffer.Orchestrators;
using Lines.Application.Features.TripRequests.DTOs;

namespace Lines.Presentation.Endpoints.Offers.Accept_Offer;

public class AcceptOfferEndPoint(BaseControllerParams<AcceptOfferRequest> dependencyCollection) : BaseController<AcceptOfferRequest, AcceptOfferResponse>(dependencyCollection)
{
    [Authorize]
    [HttpPost("offer/accept")]
    public async Task<ApiResponse<AcceptOfferResponse>> Accept([FromBody] AcceptOfferRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
            return validationResult;

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
            return HandleResult<CreatedTripForDriverDto, AcceptOfferResponse>(
              Result<CreatedTripForDriverDto>.Failure(new Error(Code: "UNAUTHORIZED", Description: "User not authenticated", Type: ErrorType.Validation)));

        var result = await _mediator.Send(new AcceptOfferOrchestrator(OfferId: request.OfferId, UserId: userId));

        return HandleResult<CreatedTripForDriverDto, AcceptOfferResponse>(result);
    }
}