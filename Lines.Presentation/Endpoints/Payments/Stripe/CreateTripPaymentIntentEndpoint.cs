using Lines.Application.Features.Trips.Payments.Commands;
using Lines.Presentation.Endpoints.Payments.Stripe;

[Authorize]
public class CreateTripPaymentIntentEndpoint
    : BaseController<CreateTripPaymentIntentRequest, CreateTripPaymentIntentResponse>
{
    public CreateTripPaymentIntentEndpoint(
        BaseControllerParams<CreateTripPaymentIntentRequest> dependencyCollection)
        : base(dependencyCollection)
    {
    }

    [HttpPost("trips/{tripId:guid}/payments/stripe/intent")]
    public async Task<ApiResponse<CreateTripPaymentIntentResponse>> CreateTripPaymentIntent(
        Guid tripId,
        [FromBody] CreateTripPaymentIntentRequest request,
        CancellationToken cancellationToken)
    {
        var passengerId = GetCurrentPassengerId();
        if (passengerId == Guid.Empty)
        {
            return ApiResponse<CreateTripPaymentIntentResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "User not authenticated", ErrorType.NotAuthenticated),
                401);
        }

        var command = new CreateTripPaymentIntentCommand(tripId, passengerId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return ApiResponse<CreateTripPaymentIntentResponse>.ErrorResponse(result.Error, 400);
        }

        var data = result.Value;

        var response = new CreateTripPaymentIntentResponse
        {
            PaymentIntentId = data.PaymentIntentId,
            ClientSecret = data.ClientSecret,
            Status = data.Status,
            Amount = data.Amount,
            Currency = data.Currency
        };

        return ApiResponse<CreateTripPaymentIntentResponse>.SuccessResponse(response);
    }
}
