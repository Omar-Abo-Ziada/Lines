using Lines.Application.Features.Passengers.GetPassengerByReferralCode.Queries;
using Lines.Domain.Models.Passengers;

namespace Lines.Application.Features.Passengers.GetPassengerByReferralCode.Orchestrators
{
    public record GetPassengerByReferralCodeOrchestrator(string referralCode) : IRequest<Result<Passenger>>;

    public class GetPassengerByReferralCodeOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetPassengerByReferralCodeOrchestrator, Result<Passenger>>(parameters)
    {
        public override async Task<Result<Passenger>> Handle(GetPassengerByReferralCodeOrchestrator request, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetPassengerByReferralCodeQuery(request.referralCode));

            return result == null ? Error.NullValue : result;
        }
    }
}
