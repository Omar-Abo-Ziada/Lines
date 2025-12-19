using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.UserReward.GetUnusedRewardByUserId.Queries
{
    public record GetUnusedRewardByUserIdQuery(Guid UserId) : IRequest<Lines.Domain.Models.User.UserReward?>;

  
    public class GetUnusedRewardByUserIdQueryHandler(RequestHandlerBaseParameters parameters , IRepository<Lines.Domain.Models.User.UserReward> _repository) :
        RequestHandlerBase<GetUnusedRewardByUserIdQuery, Lines.Domain.Models.User.UserReward?>(parameters)
    {
        public override async Task<Lines.Domain.Models.User.UserReward?> Handle(GetUnusedRewardByUserIdQuery request, CancellationToken cancellationToken)
        {
            var unusedReward = await _repository.Get(ur => ur.UserId == request.UserId && !ur.IsUsed)
                                                .SingleOrDefaultAsync();

            return unusedReward;
        }
    }
}
