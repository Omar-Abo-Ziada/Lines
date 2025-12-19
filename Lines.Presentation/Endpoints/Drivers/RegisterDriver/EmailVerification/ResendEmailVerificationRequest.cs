using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.EmailVerification;

public class ResendEmailVerificationRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
}

public class ResendEmailVerificationRequestValidator : AbstractValidator<ResendEmailVerificationRequest>
{
    public ResendEmailVerificationRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");
    }
}

public class ResendEmailVerificationResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
}
