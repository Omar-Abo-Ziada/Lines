using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Notifications.GetNotifications.DTOs
{
    public class GetNotificationsDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }

    }
}
