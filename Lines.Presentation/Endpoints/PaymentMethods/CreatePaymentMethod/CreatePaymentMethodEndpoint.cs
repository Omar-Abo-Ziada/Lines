using Lines.Application.Features.PaymentMethods.CreatePaymentMethod.Orchestrators;

namespace Lines.Presentation.Endpoints.PaymentMethods.CreatePaymentMethod
{
    public class CreatePaymentMethodEndpoint
        : BaseController<CreatePaymentMethodRequest, bool>
    {
        public CreatePaymentMethodEndpoint(
            BaseControllerParams<CreatePaymentMethodRequest> dependencyCollection
        ) : base(dependencyCollection)
        {
        }

        [HttpPost("payment-method/create")]
        public async Task<ApiResponse<bool>> HandleAsync(
            [FromBody] CreatePaymentMethodRequest request,
            CancellationToken cancellationToken)
        {
            // Validate request using shared validation method
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var userId = new Guid("EAC21BD3-32B8-477A-C04D-08DE08100D95"); //GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<bool>.ErrorResponse(Error.UnAuthorized, 401);
            }

            // Send MediatR Command
            var result = await _mediator.Send(
                new CreatePaymentMethodOrchestrator(
                    userId,
                    request.PaymentMethodId,
                    request.paymentMethodType,
                    request.isDefault
                ),
                cancellationToken
            ).ConfigureAwait(false);

            // Return response
            return ApiResponse<bool>
                .SuccessResponse(result, 200);
        }
    }
}
