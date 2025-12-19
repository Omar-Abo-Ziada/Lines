using Lines.Domain.Models.Passengers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Passengers.GetPassengerByReferralCode.Queries
{
    public record GetPassengerByReferralCodeQuery(string referralCode) : IRequest<Passenger>;

    public class GetPassengerByReferralCodeQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Passenger> _repository)
        : RequestHandlerBase<GetPassengerByReferralCodeQuery, Passenger>(parameters)
    {
        public override async Task<Passenger> Handle(GetPassengerByReferralCodeQuery request, CancellationToken cancellationToken)
        {
            var passenger = await _repository.Get(p => p.ReferralCode == request.referralCode)
                                             .FirstOrDefaultAsync(cancellationToken);
           
            return passenger;
        }
    }
}
