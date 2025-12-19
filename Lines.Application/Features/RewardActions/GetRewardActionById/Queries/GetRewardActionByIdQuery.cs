using Lines.Application.Features.RewardActions.Shared.DTOs;
using Lines.Domain.Models.User;

namespace Lines.Application.Features.RewardActions.GetRewardActionById.Queries
{
    public record GetRewardActionByIdQuery(Guid Id) : IRequest<GetRewardActionsDTO>;
    
    public class GetRewardActionByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<RewardAction> repository)
        : RequestHandlerBase<GetRewardActionByIdQuery, GetRewardActionsDTO>(parameters)
    {
        public override async Task<GetRewardActionsDTO> Handle(GetRewardActionByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
            var dto = entity.Adapt<GetRewardActionsDTO>();
            return dto;
        }
    }
}
