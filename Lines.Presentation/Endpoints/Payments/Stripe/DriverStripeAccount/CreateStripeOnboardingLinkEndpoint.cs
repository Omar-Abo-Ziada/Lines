using Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators;

namespace Lines.Presentation.Endpoints.Payments.Stripe.DriverStripeAccount
{
    [Authorize]
    public class CreateStripeOnboardingLinkEndpoint
     : BaseController<object, string>
    {
        private readonly BaseControllerParams<object> _dependencyCollection;

        public CreateStripeOnboardingLinkEndpoint(
            BaseControllerParams<object> dependencyCollection
        ) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("drivers/stripe/onboarding-link")]
        public async Task<ApiResponse<string>> CreateOnboardingLink()
        {
            var driverId = GetCurrentDriverId();
            if (driverId == Guid.Empty)
            {
                return ApiResponse<string>.Unauthorized();
            }

            var result = await _mediator.Send(
                new CreateDriverStripeOnboardingLinkOrchestrator(driverId)
            );

            return HandleResult<string, string>(result);
        }
    }
}
