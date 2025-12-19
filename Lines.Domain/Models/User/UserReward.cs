using Lines.Domain.Models.Common;
using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.User
{
    public class UserReward : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid RewardId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
        public virtual Reward Reward { get; set; }
        public virtual Guid? TripRequestId { get; set; }
        public virtual TripRequest? TripRequest { get; set; }

        public UserReward(Guid userId , Guid rewardId)
        {
            UserId = userId;
            RewardId = rewardId;
        }

        public UserReward()
        {
            
        }

        public bool IsValid(Guid userId)
        {
            return UserId == userId && !IsUsed;
        }

        public void MarkAsUsed(Guid tripRequestId)
        {
            IsUsed = true;
            UsedAt = DateTime.Now;
            TripRequestId = tripRequestId;
        }

        public void MarkAsUnUsed()
        {
            IsUsed = false;
            UsedAt = null;
            TripRequestId = null;
        }
    }
}
