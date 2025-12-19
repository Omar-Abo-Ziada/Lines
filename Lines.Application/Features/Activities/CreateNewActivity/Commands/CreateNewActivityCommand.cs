using Lines.Domain.Models.Users;

namespace Lines.Application.Features.Activities.CreateNewActivity.Commands
{
    public record CreateNewActivityCommand(Guid userId) : IRequest<Guid>;

    public class CreateNewActivityCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Activity> _repository)
        : RequestHandlerBase<CreateNewActivityCommand, Guid>(parameters)
    {
        public override async Task<Guid> Handle(CreateNewActivityCommand request, CancellationToken cancellationToken)
        {
            Activity activity = new Activity(request.userId, DateOnly.FromDateTime(DateTime.UtcNow));
            await _repository.AddAsync(activity);
            await _repository.SaveChangesAsync();

            return activity.Id;
        }
    }
}
