using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Withdrawal;

public class RegisterDriverWithdrawalRequest
{
    public string RegistrationToken { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string IBAN { get; set; } = string.Empty;
    public string SWIFT { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
}

public class RegisterDriverWithdrawalRequestValidator : AbstractValidator<RegisterDriverWithdrawalRequest>
{
    public RegisterDriverWithdrawalRequestValidator()
    {
        RuleFor(x => x.RegistrationToken)
            .NotEmpty().WithMessage("Registration token is required");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("Bank name is required")
            .MaximumLength(100).WithMessage("Bank name cannot exceed 100 characters");

        RuleFor(x => x.IBAN)
            .NotEmpty().WithMessage("IBAN is required")
            .Matches(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{4}[0-9]{7}([A-Z0-9]?){0,16}$").WithMessage("IBAN must be a valid format");

        RuleFor(x => x.SWIFT)
            .NotEmpty().WithMessage("SWIFT code is required")
            .Matches(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$").WithMessage("SWIFT code must be a valid format");

        RuleFor(x => x.AccountHolderName)
            .NotEmpty().WithMessage("Account holder name is required")
            .MaximumLength(100).WithMessage("Account holder name cannot exceed 100 characters");
    }
}
