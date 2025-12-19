using Lines.Application.Features.UserReward.GetUserRewardById.Queries;

namespace Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators
{
    public record GetUserRewardByIdOrchestrator(Guid UserRewardId) : IRequest<Result<Domain.Models.User.UserReward?>>;


    public class GetUserRewardByIdOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.User.UserReward> _repository) :
        RequestHandlerBase<GetUserRewardByIdOrchestrator, Result<Domain.Models.User.UserReward?>>(parameters)
    {
        public override async Task<Result<Domain.Models.User.UserReward?>> Handle(GetUserRewardByIdOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserRewardByIdQuery(request.UserRewardId), cancellationToken);

            return result == null ? Result<Domain.Models.User.UserReward?>.Failure(Error.NullValue) : 
                                    Result<Domain.Models.User.UserReward?>.Success(result);
        }
    }
}
