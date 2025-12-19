using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.EmailVerification;

public class VerifyEmailRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
}

public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");
        
        RuleFor(x => x.VerificationCode)
            .NotEmpty().WithMessage("Verification code is required")
            .Length(6).WithMessage("Verification code must be 6 digits")
            .Matches(@"^\d{6}$").WithMessage("Verification code must contain only digits");
    }
}

public class VerifyEmailResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
}
