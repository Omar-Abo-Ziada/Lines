using FluentValidation;
namespace Lines.Presentation.Endpoints.Feedbacks.CreateFeedback
{
    public record CreateFeedbackRequest(
                      Guid TripId,
                      int Rating,
                      string? Comment);

    public class CreateFeedbackRequestValidator : AbstractValidator<CreateFeedbackRequest>
    {
        public CreateFeedbackRequestValidator()
        {
            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}
