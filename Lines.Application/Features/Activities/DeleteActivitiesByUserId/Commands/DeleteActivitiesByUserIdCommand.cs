using Lines.Application.Interfaces;
using Lines.Domain.Models.Users;

namespace Lines.Application.Features.Activities.DeleteActivitiesByUserId.Commands
{
    public record DeleteActivitiesByUserIdCommand(Guid UserId) : IRequest<bool>;

    public class DeleteActivitiesByUserIdCommandHandler(
       RequestHandlerBaseParameters parameters,
       IRepository<Activity> repository)
       : RequestHandlerBase<DeleteActivitiesByUserIdCommand, bool>(parameters)
    {
        private readonly IRepository<Activity> _repository = repository;

        public override async Task<bool> Handle(DeleteActivitiesByUserIdCommand request, CancellationToken cancellationToken)
        {
            var activities = _repository.Get(x => x.UserId == request.UserId).ToList();
            foreach (var activity in activities)
            {
                activity.IsDeleted = true; // Mark as deleted
                await _repository.UpdateAsync(activity, cancellationToken);
            }

            return true;
        }
    }
}
