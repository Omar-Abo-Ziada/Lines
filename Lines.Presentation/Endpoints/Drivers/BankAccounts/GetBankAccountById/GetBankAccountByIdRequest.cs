using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.BankAccounts.GetBankAccountById;

public class GetBankAccountByIdRequest
{
    public Guid Id { get; set; }
}

public class GetBankAccountByIdRequestValidator : AbstractValidator<GetBankAccountByIdRequest>
{
    public GetBankAccountByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank account ID is required.");
    }
}

