using Lines.Application.Features.Activities.CreateNewActivity.Commands;
using Lines.Application.Features.Activities.GetLatestActivityByUserId.Orchestrators;
using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Orchestrators;

namespace Lines.Application.Features.Activities.CreateNewActivity.Orchestrators
{
    public record CreateNewActivityOrchestrator(Guid userId, Guid passengerId) : IRequest<Result>;

    public class CreateNewActivityOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<CreateNewActivityOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(CreateNewActivityOrchestrator request, CancellationToken cancellationToken)
        {
            // get max activity for user
            var result = await _mediator.Send(new GetLatestActivityByUserIdOrchestrator(request.userId));
        
            if(result.Value == null || result.Value.Day < DateOnly.FromDateTime(DateTime.Now))
            {
                // that user had not made activity today

                // insert record
                await _mediator.Send(new CreateNewActivityCommand(request.userId), cancellationToken);

                // get that passenger
                var passenger = await _mediator.Send(new GetPassengerByIdOrchestrator(request.passengerId));


                // check if max was yesterday >> then increment consecutiveActiveDays , else reset to 1

                if (result.Value != null && result.Value.Day == DateOnly.FromDateTime(DateTime.Now.AddDays(-1)))
                {
                    // was yesterday
                    passenger.Value.IncrementConsecutiveActiveDays();

                    // check if % 7 == 0
                    if (passenger.Value.ConsecutiveActiveDays % 7 == 0)
                    {
                        // add points to that user >> here
                        // get points to be gained on register from reward actions table
                        var dailyVisitWeekRewardActionDto = await _mediator.Send(new GetRewardActionByTypeOrchestrator(RewardActionType.DailyVisitWeek));
                        if (dailyVisitWeekRewardActionDto.IsFailure)
                        {
                            return Result<Guid>.Failure(dailyVisitWeekRewardActionDto.Error);
                        }
                        passenger.Value.AddPoints(dailyVisitWeekRewardActionDto.Value.Points);
                    }
                }
                else
                {
                    // was not yesterday
                    passenger.Value.ConsecutiveActiveDays = 1;
                }
            }
            return Result.Success();
        }
    }
}
