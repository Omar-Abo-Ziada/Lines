using Lines.Application.Features.Payments.DriverStripeAccounts.Commands;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators
{
    public record   CreateOrGetDriverStripeAccountOrchestrator(Guid DriverId)
        : IRequest<Result<string>>; // StripeAccountId

    public class CreateOrGetDriverStripeAccountOrchestratorHandler
        : RequestHandlerBase<CreateOrGetDriverStripeAccountOrchestrator, Result<string>>
    {
        private readonly IMediator _mediator;

        public CreateOrGetDriverStripeAccountOrchestratorHandler(
            RequestHandlerBaseParameters parameters,
            IMediator mediator
        ) : base(parameters)
        {
            _mediator = mediator;
        }

        public override async Task<Result<string>> Handle(
            CreateOrGetDriverStripeAccountOrchestrator request,
            CancellationToken ct)
        {
             return await _mediator.Send(
                new CreateOrGetDriverStripeAccountCommand(request.DriverId), ct);
        }
    }
}
