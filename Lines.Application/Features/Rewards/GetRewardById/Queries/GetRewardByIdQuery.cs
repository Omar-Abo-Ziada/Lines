using Lines.Domain.Models.User;

namespace Lines.Application.Features.Rewards.GetRewardById.Queries
{
    public record GetRewardByIdQuery(Guid Id) : IRequest<Reward>;
    
    public class GetRewardByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Reward> _repository) :
        RequestHandlerBase<GetRewardByIdQuery, Reward>(parameters)
    {
        public override async Task<Reward> Handle(GetRewardByIdQuery request, CancellationToken cancellationToken)
        {
            var reward = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return reward;
        }
    }
}
