using FluentValidation;

namespace Lines.Presentation.Endpoints.Otps.ResendOtp
{
    public record ResendOtpRequest(Guid? UserId, string? Email);

    public class ResendOtpRequestValidator : AbstractValidator<ResendOtpRequest>
    {
        public ResendOtpRequestValidator()
        {
            // Optional: individual rules
            RuleFor(r => r.Email)
                .EmailAddress()
                .When(r => !string.IsNullOrWhiteSpace(r.Email))
                .WithMessage("Invalid email format.");

            // At least one required  >> in confirm mail case >> userid is required , in forget password case >> email is required
            RuleFor(r => r)
                .Must(r => r.UserId.HasValue || !string.IsNullOrWhiteSpace(r.Email))
                .WithMessage("Either UserId or Email is required.");
        }
    }
}
