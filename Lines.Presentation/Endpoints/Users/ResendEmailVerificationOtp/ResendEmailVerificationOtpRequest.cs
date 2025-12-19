using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.ResendEmailVerificationOtp;

public class ResendEmailVerificationOtpRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResendEmailVerificationOtpRequestValidator : AbstractValidator<ResendEmailVerificationOtpRequest>
{
    public ResendEmailVerificationOtpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");
    }
}
