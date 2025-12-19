using Lines.Application.Features.Passengers.GetPassengerById.Queries;
using Lines.Application.Features.Users.GetUserById.Orchestrators;

namespace Lines.Application.Features.Passengers.IsReferralCodeSubmittedByPassenger.Orchestrators
{
    public record IsReferralCodeSubmittedByPassengerOrchestrator(Guid userId) : IRequest<Result<bool>>;

    public class IsReferralCodeSubmittedByPassengerOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<IsReferralCodeSubmittedByPassengerOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(IsReferralCodeSubmittedByPassengerOrchestrator request, CancellationToken cancellationToken)
        {
            var passengerAndDriverIds = await _mediator.Send(new GetPassengerAndDriverIdsByUserIdOrchestrator(request.userId));

            if (passengerAndDriverIds == null || passengerAndDriverIds.Value.PassengerId == null)
            {
                return Error.NullValue;
            }

            var passenger = await _mediator.Send(new GetPassengerByIdQuery((Guid)passengerAndDriverIds.Value.PassengerId));

            return passenger == null ? Error.NullValue : passenger.isReferralCodeSubmitted;
        }
    }


}
