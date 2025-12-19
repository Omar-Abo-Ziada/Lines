using FluentValidation;
using Lines.Presentation.Endpoints.Sectors;

namespace Lines.Presentation.Endpoints.Notifications.ReadNotifications
{
    public record ReadNotificationsRequest(Guid Id);
    public class ReadNotificationsRequestValidator : AbstractValidator<ReadNotificationsRequest>
    {
        public ReadNotificationsRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }

}
