using Lines.Domain.Enums.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.DTOs.Notifications
{
    public record DeviceTokenRequest(
    Guid UserId,
    string Token,
    string? DeviceId,
    DevicePlatform Platform,
    string? Locale,
    bool? NotificationsEnabled
      );

}
