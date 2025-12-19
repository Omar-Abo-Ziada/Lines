using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.UpdateDriverContact;

public class UpdateDriverContactRequest
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

public class UpdateDriverContactRequestValidator : AbstractValidator<UpdateDriverContactRequest>
{
    public UpdateDriverContactRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        // At least one field must be provided
        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("At least one field (Email or PhoneNumber) must be provided");
    }
}
