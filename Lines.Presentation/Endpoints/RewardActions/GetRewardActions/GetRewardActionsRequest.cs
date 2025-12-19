using FluentValidation;

namespace Lines.Presentation.Endpoints.RewardActions.GetRewardActions
{
    public record GetRewardActionsRequest(int PageNumber = 1, int PageSize = 10);

    public class GetRewardActionsRequestValidator : AbstractValidator<GetRewardActionsRequest>
    {
        public GetRewardActionsRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}
