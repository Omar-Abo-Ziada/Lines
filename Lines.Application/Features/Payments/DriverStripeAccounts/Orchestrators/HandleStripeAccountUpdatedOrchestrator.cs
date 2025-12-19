using Lines.Application.Features.Payments.DriverStripeAccounts.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators
{
    public record HandleStripeAccountUpdatedOrchestrator(
     string StripeAccountId,
     bool ChargesEnabled,
     bool PayoutsEnabled,
     bool DetailsSubmitted
 ) : IRequest<Result<bool>>;

    public class HandleStripeAccountUpdatedOrchestratorHandler
 : RequestHandlerBase<HandleStripeAccountUpdatedOrchestrator, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HandleStripeAccountUpdatedOrchestratorHandler> _logger;

        public HandleStripeAccountUpdatedOrchestratorHandler(
            RequestHandlerBaseParameters parameters,
            IMediator mediator,
            ILogger<HandleStripeAccountUpdatedOrchestratorHandler> logger
        ) : base(parameters)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<Result<bool>> Handle(
            HandleStripeAccountUpdatedOrchestrator request,
            CancellationToken ct)
        {
            _logger.LogInformation(
                "Handling Stripe account.updated for Account {StripeAccountId}",
                request.StripeAccountId);

            // 1) Update local DB
            var updateResult = await _mediator.Send(
                new UpdateDriverStripeAccountStatusCommand(
                    request.StripeAccountId,
                    request.ChargesEnabled,
                    request.PayoutsEnabled,
                    request.DetailsSubmitted
                ),
                ct);

            if (!updateResult.IsSuccess)
                return updateResult;

            // 2) (مستقبلاً) Notifications
            // 3) (مستقبلاً) Audit log
            // 4) (مستقبلاً) Enable payouts job

            return Result<bool>.Success(true);
        }
    }

}
