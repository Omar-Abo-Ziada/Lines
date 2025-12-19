using Lines.Application.Features.Drivers.GetDriverById.Orchestrators;
using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Queries;
using Lines.Application.Features.Trips.CompleteTrip.Commands;
using Lines.Application.Features.Trips.GetTripById.Orchestrators;
using Lines.Application.Features.Users.GetUserById.Orchestrators;
using Lines.Application.Features.Users.GetUserById.Queries;

namespace Lines.Application.Features.Trips.CompleteTrip.Orchestrators
{
    public record CompleteTripOrchestrator(Guid tripId, Guid userId) : IRequest<Result<bool>>;

    public class CompleteTripOrchestratorHandler(RequestHandlerBaseParameters parameters) 
                                : RequestHandlerBase<CompleteTripOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(CompleteTripOrchestrator request, CancellationToken cancellationToken)
        {
            var trip = await _mediator.Send(new GetTripByIdOrchestrator(request.tripId), cancellationToken);
            if (trip == null)
            {
                return Result<bool>.Failure(Error.NullValue);
            }

            if (trip.Value.Status != TripStatus.InProgress)
            {
                return Result<bool>.Failure(TripErrors.InvalidStatus);
            }

            var passengerAndDriverIds = await _mediator.Send(new GetPassengerAndDriverIdsByUserIdOrchestrator(request.userId));

            if (trip.Value.PassengerId != passengerAndDriverIds.Value.PassengerId)
            {
                return Result<bool>.Failure(TripErrors.PassengerMismatch);
            }

            var completeResult = await _mediator.Send(new CompleteTripCommand(trip.Value), cancellationToken);
            if (!completeResult)
            {
                return Result<bool>.Failure(new Error("400" , "Error occurred during marking the trip as completed" 
                                                      , ErrorType.Failure));
            }

            var passenger = await _mediator.Send(new GetPassengerByIdOrchestrator(trip.Value.PassengerId));
            passenger.Value.IncrementTripsCount();

            if(passenger.Value.TotalTrips % 5 == 0)
            {
                // get points to be gained on completing 5 trips
                var rewardActionDto = await _mediator.Send(new GetRewardActionByTypeOrchestrator(RewardActionType.FiveRidesBonus));

                passenger.Value.AddPoints(rewardActionDto.Value.Points);
            }

            var driver = await _mediator.Send(new GetDriverByIdOrchestrator(trip.Value.DriverId));
            driver.Value.IncrementTripsCount();

            return true;
        }
    }

}
