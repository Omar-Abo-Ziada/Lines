using Lines.Application.Features.UserReward.GetUnusedRewardByUserId.Queries;
using Lines.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.UserReward.GetUnusedRewardByUserId.Orchestrators
{
    public record GetUnusedRewardByUserIdOrchestrator(Guid UserId) : IRequest<Result<Lines.Domain.Models.User.UserReward>>;

    public class GetUnusedRewardByUserIdOrchestratorHandler(RequestHandlerBaseParameters parameters) : 
        RequestHandlerBase<GetUnusedRewardByUserIdOrchestrator, Result<Domain.Models.User.UserReward>>(parameters)
    {
        public override async Task<Result<Domain.Models.User.UserReward>> Handle(GetUnusedRewardByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            var unusedReward = await _mediator.Send(new GetUnusedRewardByUserIdQuery(request.UserId));


            return unusedReward != null ? Result<Domain.Models.User.UserReward>.Success(unusedReward)
                                        : Result<Domain.Models.User.UserReward>.Failure(Error.NullValue);
        }
    }

}
