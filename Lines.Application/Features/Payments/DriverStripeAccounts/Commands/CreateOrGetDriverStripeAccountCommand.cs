using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Models.Drivers.StripeAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Commands
{
    public record CreateOrGetDriverStripeAccountCommand(Guid DriverId)
     : IRequest<Result<string>>; // يرجع StripeAccountId

    public class CreateOrGetDriverStripeAccountCommandHandler
   : RequestHandlerBase<CreateOrGetDriverStripeAccountCommand, Result<string>>
    {
        private readonly IRepository<DriverStripeAccount> _repo;
        private readonly IPaymentGateway _stripe;

        public CreateOrGetDriverStripeAccountCommandHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<DriverStripeAccount> repo,
            IPaymentGateway stripe
        ) : base(parameters)
        {
            _repo = repo;
            _stripe = stripe;
        }

        public override async Task<Result<string>> Handle(
            CreateOrGetDriverStripeAccountCommand request,
            CancellationToken ct)
        {
            var existing = await _repo
                .Get(x => x.DriverId == request.DriverId)
                .FirstOrDefaultAsync(ct);

            if (existing != null)
                return Result<string>.Success(existing.StripeAccountId);

            var accountId = await _stripe
                .CreateExpressConnectedAccountAsync(request.DriverId, ct);

            var entity = new DriverStripeAccount(request.DriverId, accountId);
            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<string>.Success(accountId);
        }
    }

}
