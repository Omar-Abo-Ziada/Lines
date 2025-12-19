
using global::Lines.Application.Features.Payments.Stripe;
using Lines.Application.Features.Trips.Payments.Commands;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Lines.Presentation.Endpoints.Payments.Stripe
{
    
 
    [Authorize]
    public class ConfirmTripPaymentEndpoint
        : BaseController<ConfirmTripPaymentRequest, ConfirmTripPaymentResponse>
    {
        public ConfirmTripPaymentEndpoint(
            BaseControllerParams<ConfirmTripPaymentRequest> dependencyCollection)
            : base(dependencyCollection)
        {
        }

        /// <summary>
        /// Confirm Stripe PaymentIntent for a trip after client-side checkout succeeds.
        /// </summary>
        [HttpPost("trips/{tripId:guid}/payments/stripe/confirm")]
        public async Task<ApiResponse<ConfirmTripPaymentResponse>> ConfirmTripPayment(
            Guid tripId,
            [FromBody] ConfirmTripPaymentRequest request,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentPassengerId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<ConfirmTripPaymentResponse>.ErrorResponse(
                    new Error("UNAUTHORIZED", "User not authenticated", ErrorType.NotAuthenticated),
                    401);
            }

            if (string.IsNullOrWhiteSpace(request.PaymentIntentId))
            {
                return ApiResponse<ConfirmTripPaymentResponse>.ErrorResponse(
                    new Error("Payment.InvalidPaymentIntent", "PaymentIntentId is required.", ErrorType.Validation),
                    400);
            }

            var command = new ConfirmStripeTripPaymentCommand(
                TripId: tripId,
                PassengerId: userId,
                PaymentIntentId: request.PaymentIntentId
            );

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<ConfirmTripPaymentResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new ConfirmTripPaymentResponse
            {
                Success = true,
                Status = "Completed"
            };

            return ApiResponse<ConfirmTripPaymentResponse>.SuccessResponse(response);
        }
    }

}
