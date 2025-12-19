using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.Notifications.ReadNotifications.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;

namespace Lines.Presentation.Endpoints.Notifications.ReadNotifications;

public class ReadNotificationsResponse
{
    public ReadNotificationsDTO Notification { get; set; } = new();
}
