using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;

namespace Lines.Presentation.Endpoints.Notifications.GetNotifications;

public class GetNotificationsResponse
{
    public List<GetNotificationsDTO> Notifications { get; set; } = new();
}
