using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.UserReward.GetUserRewardById.Queries
{
    public record GetUserRewardByIdQuery(Guid UserRewardId) : IRequest<Domain.Models.User.UserReward?>;

    public class GetUserRewardByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.User.UserReward> _repository) :
        RequestHandlerBase<GetUserRewardByIdQuery, Domain.Models.User.UserReward?>(parameters)
    {
        public override async Task<Domain.Models.User.UserReward?> Handle(GetUserRewardByIdQuery request, CancellationToken cancellationToken)
        {
            var userReward = await _repository.Get(ur => ur.Id == request.UserRewardId)
                                              .Include(ur => ur.Reward)
                                              .SingleOrDefaultAsync();

            return userReward;
        }
    }
}
