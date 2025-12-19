using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Users;
using MediatR;

namespace Lines.Application.Features.Notifications.DeleteNotificationsByUserId.Commands
{
    public record DeleteNotificationsByUserIdCommand(Guid UserId) : IRequest<bool>;

    public class DeleteNotificationsByUserIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<Notification> repository)
         : RequestHandlerBase<DeleteNotificationsByUserIdCommand, bool>(parameters)
    {
        private readonly IRepository<Notification> _repository = repository;

        public override async Task<bool> Handle(DeleteNotificationsByUserIdCommand request, CancellationToken cancellationToken)
        {
            var notifications = _repository.Get(x => x.UserId == request.UserId).ToList();

            foreach (var notification in notifications)
            {
                notification.IsDeleted = true; 
                await _repository.UpdateAsync(notification, cancellationToken);
            }

            return true;
        }
    }
} 