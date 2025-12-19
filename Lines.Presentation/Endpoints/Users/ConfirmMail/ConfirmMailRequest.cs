using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.ConfirmMail
{
    public record ConfirmMailRequest(Guid UserId, string Otp);

    public class ConfirmMailRequestValidator : AbstractValidator<ConfirmMailRequest>
    {
        public ConfirmMailRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(r => r.Otp)
                .NotEmpty().WithMessage("Otp is required.")
                .Length(6).WithMessage("Otp must be exactly 6 characters long.");
        }
    }
}
