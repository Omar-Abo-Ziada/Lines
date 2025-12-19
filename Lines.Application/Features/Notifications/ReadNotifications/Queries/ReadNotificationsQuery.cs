using Lines.Application.Common;
using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.Notifications.ReadNotifications.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Domain.Enums;
using Lines.Domain.Models.TermsAndConditions;
using Lines.Domain.Models.Users;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Notifications.ReadNotifications.Queries
{
    public record ReadNotificationsQuery(Guid Id) : IRequest<ReadNotificationsDTO>;
    public class ReadNotificationsHandler(
      RequestHandlerBaseParameters parameters,
      IRepository<Notification> notificationsRepository)
      : RequestHandlerBase<ReadNotificationsQuery, ReadNotificationsDTO>(parameters)
    {   
        public override async Task<ReadNotificationsDTO> Handle(ReadNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notification = notificationsRepository
                .Get(n=>n.Id==request.Id)
                .FirstOrDefault();
            notification.IsRead= true;

            await notificationsRepository.UpdateAsync(notification);

            notificationsRepository.SaveChanges();

            var result =  new ReadNotificationsDTO
                {
                    Id= notification.Id,
                    Title= notification.Title,
                    Message = notification.Message,
                    CreatedDate = notification.CreatedDate,
                    IsRead = notification.IsRead,
                };

            return result;
        }
    }
}
