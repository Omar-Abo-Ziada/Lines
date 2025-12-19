using Lines.Application.Common;
using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Domain.Enums;
using Lines.Domain.Models.TermsAndConditions;
using Lines.Domain.Models.Users;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace Lines.Application.Features.Notifications.GetNotifications.Queries
{
    public record GetNotificationsQuery(Guid UserId, bool? IsRead) : IRequest<List<GetNotificationsDTO>>;
    public class GetNotificationsHandler(
      RequestHandlerBaseParameters parameters,
      IRepository<Notification> notificationsRepository)
      : RequestHandlerBase<GetNotificationsQuery, List<GetNotificationsDTO>>(parameters)
    {   
        public override async Task<List<GetNotificationsDTO>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = notificationsRepository
                .Get(n=>n.UserId==request.UserId);

            if (request.IsRead==true)
                notifications=notifications.Where(n=>n.IsRead==true);
            if (request.IsRead == false)
                notifications = notifications.Where(n => n.IsRead == false);

            var result = await notifications
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new GetNotificationsDTO
                {
                    Id=n.Id,
                    Message = n.Message,
                    CreatedDate = n.CreatedDate,
                    IsRead = n.IsRead,
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return result;
        }
    }
}
