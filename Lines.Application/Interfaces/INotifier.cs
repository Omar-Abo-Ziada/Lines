using Lines.Application.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Interfaces
{
    public interface IFcmNotifier
    {
        Task<NotificationResponse> SendToUserAsync(Guid userId, string title, string body, object data = null);
        Task<NotificationResponse> SendToManyAsync(IEnumerable<string> tokens, string title, string body, object data = null);
    }
}
