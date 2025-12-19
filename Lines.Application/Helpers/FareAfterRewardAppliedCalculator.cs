namespace Lines.Application.Helpers
{
    public static class FareAfterRewardAppliedCalculator
    {
        public static decimal Calculate(decimal Fare, decimal discountPercentage, decimal maxValue)
        {
            if (Fare <= 0)
                throw new InvalidOperationException("Cannot apply discount on a trip with no fare.");

            if (discountPercentage <= 0)
                throw new ArgumentException("Discount amount must be greater than zero.");

            if (maxValue <= 0)
                throw new ArgumentException("Max value must be greater than zero.");

            // Calculate raw discount (e.g., 0.1m = 10%)
            var discountAmount = Fare * discountPercentage;

            // Cap the discount at maxValue
            var appliedDiscount = Math.Min(discountAmount, maxValue);

            // Apply discount safely
            decimal FareAfterRewardApplied = Math.Max(0, Fare - appliedDiscount);
            return FareAfterRewardApplied;
        }
    }
}
