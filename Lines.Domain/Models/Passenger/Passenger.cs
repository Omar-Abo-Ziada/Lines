using Lines.Domain.Models.Trips;
using Lines.Domain.Value_Objects;

namespace Lines.Domain.Models.Passengers
{
    public class Passenger : Users.User
    {
        public string ReferralCode { get; set; }  // his own referral code to be shared with his friends
        public bool isReferralCodeSubmitted { get; set; } // if he was referred by someone else 
        public int ConsecutiveActiveDays { get; set; }
        public virtual List<TripRequest>? TripRequests { get; set; }
        public virtual List<Trip>? Trips { get; set; }
    

        // Constructor
        public Passenger() : base() { }

        public Passenger(Guid id, string firstName, string lastName, Email email, PhoneNumber phoneNumber, string referralCode)
            : base(id, firstName, lastName, email, phoneNumber)
        {
          ReferralCode = referralCode;
        }

        public void IncrementConsecutiveActiveDays()
        {
            ConsecutiveActiveDays++;
        }

        public void DeductRewardPoints(int rewardPoints)
        {
            Points -= rewardPoints;
        }

        private void ValidatePassengerDetails(string firstName, string lastName, Email email, PhoneNumber phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty");
        }
    }
}