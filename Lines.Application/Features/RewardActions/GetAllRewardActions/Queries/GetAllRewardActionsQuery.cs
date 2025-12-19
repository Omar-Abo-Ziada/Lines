using Application.Common.Helpers;
using Lines.Application.Features.RewardActions.Shared.DTOs;
using Lines.Domain.Models.User;

namespace Lines.Application.Features.Rewards.GetRewardActions.Queries
{
    public record GetAllRewardActionsQuery(int pageNumber = 1, int pageSize = 10) : IRequest<PagingDto<GetRewardActionsDTO>>;

    public class GetRewardActionsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<RewardAction> repository)
        : RequestHandlerBase<GetAllRewardActionsQuery, PagingDto<GetRewardActionsDTO>>(parameters)
    {
        public override async Task<PagingDto<GetRewardActionsDTO>> Handle(GetAllRewardActionsQuery request, CancellationToken cancellationToken)
        {
            var query = repository.Get().ProjectToType<GetRewardActionsDTO>();
            var result = await PagingHelper.CreateAsync(query, request.pageNumber, request.pageSize);
            return result;
        }
    } 
}
