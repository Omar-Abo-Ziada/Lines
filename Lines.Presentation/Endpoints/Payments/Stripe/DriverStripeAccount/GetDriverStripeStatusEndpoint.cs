using Lines.Application.Features.Payments.DriverStripeAccounts.Commands;

namespace Lines.Presentation.Endpoints.Payments.Stripe.DriverStripeAccount
{
    public class GetDriverStripeStatusEndpoint
     : BaseController<object, DriverStripeStatusDto>
    {
        private readonly BaseControllerParams<object> _dependencyCollection;

        public GetDriverStripeStatusEndpoint(
            BaseControllerParams<object> dependencyCollection
        ) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpGet("drivers/stripe/status")]
        public async Task<ApiResponse<DriverStripeStatusDto>> GetStatus()
        {
            var driverId = GetCurrentDriverId();
            if (driverId == Guid.Empty)
            {
                return ApiResponse<DriverStripeStatusDto>.Unauthorized();
            }

            var result = await _mediator.Send(
                new GetDriverStripeStatusQuery(driverId)
            );

            return HandleResult<DriverStripeStatusDto, DriverStripeStatusDto>(result);
        }
    }
}
