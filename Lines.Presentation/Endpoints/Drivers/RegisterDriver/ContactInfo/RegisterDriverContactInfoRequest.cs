using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.ContactInfo;

public class RegisterDriverContactInfoRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class RegisterDriverContactInfoRequestValidator : AbstractValidator<RegisterDriverContactInfoRequest>
{
    public RegisterDriverContactInfoRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");
    }
}
