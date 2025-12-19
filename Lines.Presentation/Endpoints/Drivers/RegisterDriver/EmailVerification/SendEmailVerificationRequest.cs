using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.EmailVerification;

public class SendEmailVerificationRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
}

public class SendEmailVerificationRequestValidator : AbstractValidator<SendEmailVerificationRequest>
{
    public SendEmailVerificationRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");
    }
}

public class SendEmailVerificationResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
}
