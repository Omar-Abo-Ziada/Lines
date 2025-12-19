using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.VerifyEmailOtp;

public class VerifyEmailOtpRequest
{
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}

public class VerifyEmailOtpRequestValidator : AbstractValidator<VerifyEmailOtpRequest>
{
    public VerifyEmailOtpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required")
            .Length(4, 6).WithMessage("OTP must be between 4 and 6 characters");
    }
}
