using Lines.Domain.Models.Drivers.StripeAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Commands
{
    public record UpdateDriverStripeAccountStatusCommand(
      string StripeAccountId,
      bool ChargesEnabled,
      bool PayoutsEnabled,
      bool DetailsSubmitted
  ) : IRequest<Result<bool>>;


    public class UpdateDriverStripeAccountStatusCommandHandler
 : RequestHandlerBase<UpdateDriverStripeAccountStatusCommand, Result<bool>>
    {
        private readonly IRepository<DriverStripeAccount> _repo;

        public UpdateDriverStripeAccountStatusCommandHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<DriverStripeAccount> repo
        ) : base(parameters)
        {
            _repo = repo;
        }

        public override async Task<Result<bool>> Handle(
            UpdateDriverStripeAccountStatusCommand request,
            CancellationToken ct)
        {
            var account = await _repo
                .Get(x => x.StripeAccountId == request.StripeAccountId)
                .FirstOrDefaultAsync(ct);

            if (account == null)
                return Result<bool>.Success(true); // Stripe retries otherwise

            account.UpdateStatus(
                request.ChargesEnabled,
                request.PayoutsEnabled,
                request.DetailsSubmitted
            );

            await _repo.UpdateAsync(account, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<bool>.Success(true);
        }
    }

}
