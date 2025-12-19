using FluentValidation;

namespace Lines.Presentation.Endpoints.RewardActions.UpdateRewardActions
{
    public record UpdateRewardActionsRequest(Guid Id, int NewPoints, string Name);

    public class UpdateRewardActionsRequestValidator : AbstractValidator<UpdateRewardActionsRequest>
    {
        public UpdateRewardActionsRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.NewPoints)
                .GreaterThan(0).WithMessage("NewPoints must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }

}
