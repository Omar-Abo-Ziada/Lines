using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.DTOs.Notifications
{
    public class NotificationResponse
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> InvalidTokens { get; set; } = new();
    }
}
