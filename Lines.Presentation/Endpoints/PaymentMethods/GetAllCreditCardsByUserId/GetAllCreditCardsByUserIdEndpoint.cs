using Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.Orchestrators;
using Mapster;

namespace Lines.Presentation.Endpoints.PaymentMethods.GetAllCreditCardsByUserId
{
    public class GetAllCreditCardsByUserIdEndpoint
        : BaseController<GetAllCreditCardsByUserIdRequest, List<GetAllCreditCardsByUserIdResponse>>
    {
        public GetAllCreditCardsByUserIdEndpoint(
            BaseControllerParams<GetAllCreditCardsByUserIdRequest> dependencyCollection
        ) : base(dependencyCollection)
        {
        }

        [HttpGet("credit-cards")]
        public async Task<ApiResponse<List<GetAllCreditCardsByUserIdResponse>>> HandleAsync(
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<List<GetAllCreditCardsByUserIdResponse>>
                    .ErrorResponse(Error.UnAuthorized, 401);
            }

            var result = await _mediator.Send(new GetAllCreditCardsByUserIdOrchestrator(userId), cancellationToken)
                                        .ConfigureAwait(false);

            if (result.IsFailure)
            {
                return ApiResponse<List<GetAllCreditCardsByUserIdResponse>>
                    .ErrorResponse(result.Error, 400);
            }

            var response = result.Value.Adapt<List<GetAllCreditCardsByUserIdResponse>>();

            return ApiResponse<List<GetAllCreditCardsByUserIdResponse>>
                .SuccessResponse(response, 200);

        }
    }
}
