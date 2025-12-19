using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.UpdateBankAccount;

public class UpdateBankAccountRequest
{
    public string BankName { get; set; } = string.Empty;
    public string IBAN { get; set; } = string.Empty;
    public string SWIFT { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public string? AccountNumber { get; set; }
    public string? BranchName { get; set; }
    public bool IsPrimary { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? CVV { get; set; }
}

public class UpdateBankAccountRequestValidator : AbstractValidator<UpdateBankAccountRequest>
{
    public UpdateBankAccountRequestValidator()
    {
        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("Bank name is required.")
            .MaximumLength(100).WithMessage("Bank name cannot exceed 100 characters.");

        RuleFor(x => x.IBAN)
            .NotEmpty().WithMessage("IBAN is required.")
            .MaximumLength(34).WithMessage("IBAN cannot exceed 34 characters.")
            .MinimumLength(15).WithMessage("IBAN must be at least 15 characters.");

        RuleFor(x => x.SWIFT)
            .NotEmpty().WithMessage("SWIFT code is required.")
            .Length(8, 11).WithMessage("SWIFT code must be 8 or 11 characters.");

        RuleFor(x => x.AccountHolderName)
            .NotEmpty().WithMessage("Account holder name is required.")
            .MaximumLength(100).WithMessage("Account holder name cannot exceed 100 characters.");

        // Optional fields
        RuleFor(x => x.AccountNumber)
            .MaximumLength(50).WithMessage("Account number cannot exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.AccountNumber));

        RuleFor(x => x.BranchName)
            .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.BranchName));

        RuleFor(x => x.CardNumber)
            .MaximumLength(19).WithMessage("Card number cannot exceed 19 characters.")
            .When(x => !string.IsNullOrEmpty(x.CardNumber));

        RuleFor(x => x.ExpiryDate)
            .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$").WithMessage("Expiry date must be in MM/YY format.")
            .When(x => !string.IsNullOrEmpty(x.ExpiryDate));

        RuleFor(x => x.CVV)
            .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits.")
            .When(x => !string.IsNullOrEmpty(x.CVV));
    }
}

