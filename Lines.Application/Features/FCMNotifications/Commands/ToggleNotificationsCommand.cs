using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.FCMNotifications.Commands
{
    public record ToggleNotificationsCommand(Guid UserId, bool Enabled) : IRequest<Result<Unit>>;

}
