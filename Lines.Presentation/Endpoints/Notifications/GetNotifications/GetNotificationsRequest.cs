using FluentValidation;
using Lines.Presentation.Endpoints.Sectors;

namespace Lines.Presentation.Endpoints.Notifications.GetNotifications
{
    public record GetNotificationsRequest ( bool? IsRead);
    public class GetNotificationsRequestValidator : AbstractValidator<GetNotificationsRequest>
    {
        public GetNotificationsRequestValidator()
        {
        }
    }

}
