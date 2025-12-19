using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.UserReward.GetUnusedRewardByUserId.Queries;
using Lines.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Rewards.GetRewardByUserRewardId.Queries
{
    public record GetRewardByUserRewardIdQuery(Guid UserRewardID) : IRequest<Reward>;

    public class GetRewardByUserRewardIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Lines.Domain.Models.User.UserReward> _repository) :
      RequestHandlerBase<GetRewardByUserRewardIdQuery, Reward>(parameters)
    {
        public override async Task<Reward> Handle(GetRewardByUserRewardIdQuery request, CancellationToken cancellationToken)
        {
            var f = _repository.Get(x => x.Id == request.UserRewardID).Select(x => x.Reward);
            var g = (f.Provider.GetType().Name);

            var reward=await  _repository.Get(x=> x.Id == request.UserRewardID).Select(x=>x.Reward).FirstOrDefaultAsync();
      
            return reward;


        }
    }
}
