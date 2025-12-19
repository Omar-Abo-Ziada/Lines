using FluentValidation;

namespace Lines.Presentation.Endpoints.Wallets.TopUpWallet;

public record TopUpWalletRequest(decimal Amount);

public class TopUpWalletRequestValidator : AbstractValidator<TopUpWalletRequest>
{
    public TopUpWalletRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.")
            .LessThanOrEqualTo(10000)
            .WithMessage("Amount cannot exceed 10,000 per transaction.");
    }
}

