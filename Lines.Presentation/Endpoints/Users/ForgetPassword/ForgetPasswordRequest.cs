using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.ForgetPassword
{
    public record ForgetPasswordRequest(string email);

    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(r => r.email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }
    }
}
