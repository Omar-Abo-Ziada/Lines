using Lines.Application.Features.Rewards.Shared.DTOs;
using Lines.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Rewards.GetAllRewards.Queries
{
    public record GetAllRewardsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagingDto<GetRewardDTO>>;

    public class GetAllRewardsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Reward> _repository)
                    : RequestHandlerBase<GetAllRewardsQuery, PagingDto<GetRewardDTO>>(parameters)
    {
        public override async Task<PagingDto<GetRewardDTO>> Handle(GetAllRewardsQuery request, CancellationToken cancellationToken)
        {

           var rewards = await _repository.Get().Skip((request.PageNumber - 1) * request.PageSize)
                                     .Take(request.PageSize)
                                     .ProjectToType<GetRewardDTO>()
                                     .ToListAsync(cancellationToken);

            return new PagingDto<GetRewardDTO>
            {
                Items = rewards,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = rewards.Count()
            };
        }
    }



}
