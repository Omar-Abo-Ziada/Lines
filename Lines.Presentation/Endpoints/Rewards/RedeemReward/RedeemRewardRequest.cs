namespace Lines.Presentation.Endpoints.Rewards.RedeemReward
{
    public record RedeemRewardRequest(Guid RewardId);
   
    public class RedeemRewardRequestValidator : AbstractValidator<RedeemRewardRequest>
    {
        public RedeemRewardRequestValidator()
        {
            RuleFor(x => x.RewardId)
              .NotEmpty()
              .WithMessage("Reward Id must not be empty.");
        }
    }
}
