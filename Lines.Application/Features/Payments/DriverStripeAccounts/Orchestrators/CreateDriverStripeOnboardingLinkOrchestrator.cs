using Lines.Application.Features.Payments.DriverStripeAccounts.Commands;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators
{
    public record CreateDriverStripeOnboardingLinkOrchestrator(Guid DriverId)
        : IRequest<Result<string>>; // URL

    public class CreateDriverStripeOnboardingLinkOrchestratorHandler
        : RequestHandlerBase<CreateDriverStripeOnboardingLinkOrchestrator, Result<string>>
    {
        private readonly IMediator _mediator;

        public CreateDriverStripeOnboardingLinkOrchestratorHandler(
            RequestHandlerBaseParameters parameters,
            IMediator mediator
        ) : base(parameters)
        {
            _mediator = mediator;
        }

        public override async Task<Result<string>> Handle(
            CreateDriverStripeOnboardingLinkOrchestrator request,
            CancellationToken ct)
        {
            return await _mediator.Send(
                new CreateDriverStripeOnboardingLinkCommand(request.DriverId),
                ct);
        }
    }
}
