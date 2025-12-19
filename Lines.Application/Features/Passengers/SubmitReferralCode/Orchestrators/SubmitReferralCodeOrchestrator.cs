using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Features.Passengers.GetPassengerByReferralCode.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators;
using Lines.Application.Features.Users.GetUserById.Orchestrators;

namespace Lines.Application.Features.Passengers.SubmitReferralCode.Orchestrators
{
    public record SubmitReferralCodeOrchestrator(Guid userId , string referralCode) : IRequest<Result>;

    public class SubmitReferralCodeOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<SubmitReferralCodeOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(SubmitReferralCodeOrchestrator request, CancellationToken cancellationToken)
        {
            var passengerAndDriverIds = await _mediator.Send(new GetPassengerAndDriverIdsByUserIdOrchestrator(request.userId));

            if (passengerAndDriverIds.IsFailure || passengerAndDriverIds.Value.PassengerId == null)
            {
                return Error.NullValue;
            }

            var passenger = await _mediator.Send(new GetPassengerByIdOrchestrator((Guid)passengerAndDriverIds.Value.PassengerId));

            if (passenger.IsFailure)
            {
                return Error.NullValue;
            }

            if(passenger.Value.isReferralCodeSubmitted)
            {
                return Result.Failure(PassengerErrors.ReferralCodeAlreadySubmitted());
            }

            var referrerPassenger = await _mediator.Send(new GetPassengerByReferralCodeOrchestrator(request.referralCode));

            if (referrerPassenger.IsFailure)
            {
                return Error.NullValue;
            }

            if(referrerPassenger.Value.Id == passenger.Value.Id)
            {
                return Result.Failure(PassengerErrors.CannotReferYourself());
            }

            // get points to be gained on referring a friend
            var rewardActionDto = await _mediator.Send(new GetRewardActionByTypeOrchestrator(RewardActionType.ReferFriend));
            referrerPassenger.Value.AddPoints(rewardActionDto.Value.Points);

            passenger.Value.isReferralCodeSubmitted = true;

            return Result.Success();
        }
    }

}
