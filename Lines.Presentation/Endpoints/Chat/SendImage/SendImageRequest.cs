using FluentValidation;

namespace Lines.Presentation.Endpoints.Chat.SendImage
{
    public record SendImageRequest(Guid TripId , string? Message , IFormFile Image);

    public class SendImageRequestValidator : AbstractValidator<SendImageRequest>
    {
        public SendImageRequestValidator()
        {
            RuleFor(x => x.TripId)
                .NotEmpty()
                .WithMessage("Trip ID is required");

            RuleFor(x => x.Message)
                .MaximumLength(1000)
                .WithMessage("Message cannot exceed 1000 characters");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Image is required");
        }

    }
}
