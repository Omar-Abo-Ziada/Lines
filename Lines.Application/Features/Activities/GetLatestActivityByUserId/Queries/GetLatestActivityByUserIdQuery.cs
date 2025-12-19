using Lines.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Activities.GetLatestActivityByUserId.Queries
{
    public record GetLatestActivityByUserIdQuery(Guid userId) : IRequest<Activity?>;

    public class GetLatestActivityByUserIdHandler(RequestHandlerBaseParameters parameters, IRepository<Activity> _repository)
        : RequestHandlerBase<GetLatestActivityByUserIdQuery, Activity?>(parameters)
    {
        public override async Task<Activity?> Handle(GetLatestActivityByUserIdQuery request, CancellationToken cancellationToken)
        {
            var activity = await _repository
                .Get(n => n.UserId == request.userId)
                .OrderByDescending(n => n.Day)
                .FirstOrDefaultAsync();

            return activity;
        }
    }
}
