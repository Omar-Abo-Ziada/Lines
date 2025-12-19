using Lines.Application.Features.RewardActions.GetRewardActionByType.Queries;

namespace Lines.Application.Features.Drivers;
public record RegisterDriverOrchestrator(string firstName, string lastName, string email, string phoneNumber, string password, bool isNotifiedForOnlyTripsAboveMyPrice)
                                            : IRequest<Result<bool>>;

public class RegisterDriverOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<RegisterDriverOrchestrator, Result<bool>>(parameters)
{
    public override async Task<Result<bool>> Handle(RegisterDriverOrchestrator request, CancellationToken cancellationToken)
    {
        // get points to be gained on register from reward actions table
        var rewardActionDto = await _mediator.Send(new GetRewardActionByTypeQuery(RewardActionType.CompleteProfile));

        var registerResult = await _mediator.Send(new RegisterDriverCommand(request.firstName, request.lastName, request.email, request.phoneNumber, request.password
                                                   , request.isNotifiedForOnlyTripsAboveMyPrice, rewardActionDto.Points), cancellationToken);
        if (!registerResult.Succeeded)
        {
            return Result<bool>.Failure(DriverErrors.RegisterDriverError(string.Join(" , ",registerResult.Errors)));
        }
     
        return Result<bool>.Success(true);
    }
}