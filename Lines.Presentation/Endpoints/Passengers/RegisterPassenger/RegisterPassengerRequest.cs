using FluentValidation;

namespace Lines.Presentation.Endpoints.Passengers;

public record RegisterPassengerRequest(string FirstName, string LastName, string PhoneNumber, string Email, string Password, string? ReferralCode);

public class RegisterPassengerRequestValidator : AbstractValidator<RegisterPassengerRequest>
{
    public RegisterPassengerRequestValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(r => r.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format.");

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");  // these validations must match the validations in value object for email and phone number in domain layer

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(r => r.ReferralCode)
            .MaximumLength(10).WithMessage("Referral code cannot exceed 10 characters.");
    }
} 