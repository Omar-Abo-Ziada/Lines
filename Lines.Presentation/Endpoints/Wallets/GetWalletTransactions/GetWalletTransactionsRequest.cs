using FluentValidation;

namespace Lines.Presentation.Endpoints.Wallets.GetWalletTransactions;

public record GetWalletTransactionsRequest(int Page, int PageSize);

public class GetWalletTransactionsRequestValidator : AbstractValidator<GetWalletTransactionsRequest>
{
    public GetWalletTransactionsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than zero.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");
    }
}

