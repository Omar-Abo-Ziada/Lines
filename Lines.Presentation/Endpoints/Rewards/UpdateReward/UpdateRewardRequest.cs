using FluentValidation;

namespace Lines.Presentation.Endpoints.Rewards.UpdateReward
{
    public record UpdateRewardRequest(Guid Id , string Title, string? Description, int PointsRequired, decimal DiscountPercentage, decimal MaxValue);

    public class UpdateRewardRequestValidator : AbstractValidator<UpdateRewardRequest>
    {
        public UpdateRewardRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id must not be empty.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title must not be empty.")
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.PointsRequired)
                .GreaterThan(0)
                .WithMessage("Points required must be greater than 0.");

            RuleFor(x => x.DiscountPercentage)
                .GreaterThan(0)
                .WithMessage("Discount percentage must be greater than 0.");

            RuleFor(x => x.MaxValue)
                .GreaterThan(0)
                .WithMessage("Max value must be greater than 0.");
        }
    }
}
