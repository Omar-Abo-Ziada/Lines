using FluentValidation;

namespace Lines.Presentation.Endpoints.Chat.SendMessage
{
    public record SendMessageRequest(Guid TripId , string Message);
    

    public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidator()
        {
            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");
        }
    }
}
