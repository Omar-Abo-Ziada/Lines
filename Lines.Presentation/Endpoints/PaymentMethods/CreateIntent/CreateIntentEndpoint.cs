using Lines.Application.Features.PaymentMethods.CreateIntent.Orchestrators;
using Lines.Domain.Constants;

namespace Lines.Presentation.Endpoints.PaymentMethods.CreateIntent
{
    //[Authorize]
    public class CreateIntentEndpoint
        : BaseController<CreateIntentRequest, CreateIntentResponse>
    {
        private readonly BaseControllerParams<CreateIntentRequest> _dependencyCollection;

        public CreateIntentEndpoint(
            BaseControllerParams<CreateIntentRequest> dependencyCollection
        ) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("payment-method/create-intent")]  // post method as it gets the client secret from stripe then save it in db
        public async Task<ApiResponse<CreateIntentResponse>> HandleAsync(CreateIntentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var userId = new Guid("EAC21BD3-32B8-477A-C04D-08DE08100D95");  // GetCurrentUserId();
            var role = Roles.Passenger; //GetCurrentUserRole();
            var result = await _mediator.Send(
                new CreateIntentOrchestrator(userId, role, request.paymentMethodType),
                cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<CreateIntentResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new CreateIntentResponse(result.Value);
            return ApiResponse<CreateIntentResponse>.SuccessResponse(response);
        }
    }
}
