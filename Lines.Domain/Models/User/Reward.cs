using Lines.Domain.Models.Common;
using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.User
{
    public class Reward : BaseModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int PointsRequired { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal MaxValue { get; set; }
        public virtual ICollection<UserReward> UserRewards { get; set; }
        public Reward(string title , decimal discountPercentage , decimal maxValue)
        {
            Title = title;
            DiscountPercentage = discountPercentage;
            MaxValue = maxValue;
        }
        // Parameterless constructor for EF
        public Reward() { }
    }
}
