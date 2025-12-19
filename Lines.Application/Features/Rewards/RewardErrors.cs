namespace Lines.Application.Features.Rewards
{
    public static class RewardErrors
    {
        public static Error UnusedRewardExistsError() => new Error("REWARD.FailedToRedeem", "You can't redeem new reward as you have old unused one.", ErrorType.Validation);
        public static Error MorePointsRequiredError() => new Error("REWARD.FailedToRedeem", "You do not have enough points to redeem this reward.", ErrorType.Validation);
    }
}
