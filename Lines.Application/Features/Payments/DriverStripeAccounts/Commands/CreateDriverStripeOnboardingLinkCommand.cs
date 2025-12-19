using Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators;
using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Models.Drivers.StripeAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Commands
{
    public record CreateDriverStripeOnboardingLinkCommand(Guid DriverId)
     : IRequest<Result<string>>; // يرجع URL

    public class CreateDriverStripeOnboardingLinkCommandHandler
    : RequestHandlerBase<CreateDriverStripeOnboardingLinkCommand, Result<string>>
    {
        private readonly IRepository<DriverStripeAccount> _repo;
        private readonly IPaymentGateway _stripe;

        public CreateDriverStripeOnboardingLinkCommandHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<DriverStripeAccount> repo,
            IPaymentGateway stripe
        ) : base(parameters)
        {
            _repo = repo;
            _stripe = stripe;
        }

        public override async Task<Result<string>> Handle(
    CreateDriverStripeOnboardingLinkCommand request,
    CancellationToken ct)
        {
            // ✅ 1. Ensure account exists (create if needed)
            var accountIdResult = await _mediator.Send(
                new CreateOrGetDriverStripeAccountOrchestrator(request.DriverId),
                ct
            );

            if (!accountIdResult.IsSuccess)
                return Result<string>.Failure(accountIdResult.Error);

            var accountId = accountIdResult.Value;

            // ✅ 2. Create onboarding link
            var url = await _stripe.CreateAccountOnboardingLinkAsync(
                accountId,
                "https://yourapp.com/stripe/refresh",
                "https://yourapp.com/stripe/return",
                ct
            );

            return Result<string>.Success(url);
        }


        //public override async Task<Result<string>> Handle(
        //    CreateDriverStripeOnboardingLinkCommand request,
        //    CancellationToken ct)
        //{
        //    var account = await _repo
        //        .Get(x => x.DriverId == request.DriverId)
        //        .FirstOrDefaultAsync(ct);

        //    if (account == null)
        //        return Result<string>.Failure(
        //            Error.Create("Stripe.AccountNotFound", "Driver has no Stripe account."));

        //    var url = await _stripe.CreateAccountOnboardingLinkAsync(
        //        account.StripeAccountId,
        //        "https://yourapp.com/stripe/refresh",
        //        "https://yourapp.com/stripe/return",
        //        ct);

        //    return Result<string>.Success(url);
        //}
    }

}
