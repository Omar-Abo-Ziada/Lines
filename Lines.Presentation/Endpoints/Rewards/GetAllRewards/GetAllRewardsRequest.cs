using FluentValidation;

namespace Lines.Presentation.Endpoints.Rewards.GetAllRewards
{
    public record GetAllRewardsRequest(int PageNumber = 1, int PageSize = 10);


    public class GetAllRewardsRequestValidator : AbstractValidator<GetAllRewardsRequest>
    {
        public GetAllRewardsRequestValidator()
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
