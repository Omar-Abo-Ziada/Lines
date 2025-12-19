using Lines.Application.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.FCMNotifications.Commands
{
    public record RegisterOrUpdateTokenCommand(DeviceTokenRequest Request)
     : IRequest<Result<bool>>;
}
