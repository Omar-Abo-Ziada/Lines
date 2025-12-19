using Lines.Domain.Models.Drivers.StripeAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Commands
{
    public record GetDriverStripeStatusQuery(Guid DriverId)
     : IRequest<Result<DriverStripeStatusDto>>;



    public class GetDriverStripeStatusQueryHandler
    : RequestHandlerBase<GetDriverStripeStatusQuery, Result<DriverStripeStatusDto>>
    {
        private readonly IRepository<DriverStripeAccount> _repo;

        public GetDriverStripeStatusQueryHandler(
            RequestHandlerBaseParameters parameters,
            IRepository<DriverStripeAccount> repo
        ) : base(parameters)
        {
            _repo = repo;
        }

        public override async Task<Result<DriverStripeStatusDto>> Handle(
            GetDriverStripeStatusQuery request,
            CancellationToken ct)
        {
            var account = await _repo
                .Get(x => x.DriverId == request.DriverId)
                .FirstOrDefaultAsync(ct);

            if (account == null)
                return Result<DriverStripeStatusDto>.Failure(
                    Error.Create("Stripe.AccountNotFound", "Stripe account not found.")
                );

            return Result<DriverStripeStatusDto>.Success(
                new DriverStripeStatusDto(
                    account.ChargesEnabled,
                    account.PayoutsEnabled,
                    account.DetailsSubmitted
                )
            );
        }
    }

}
