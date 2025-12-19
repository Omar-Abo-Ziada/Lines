using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Trips
{

    public class Feedback : BaseModel
    {
        public Guid TripId { get;  set; }
        public virtual Trip Trip { get;  set; }
        public Guid FromUserId { get;  set; }
        public Guid ToUserId { get;  set; }
        public int Rating { get;  set; } // e.g., 1-5 stars
        public string? Comment { get;  set; }


        // Constructor
        public Feedback(Guid tripId, Guid fromUserId, Guid toUserId, int rating, string? comment)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("TripId must be valid.", nameof(tripId));
            if (fromUserId == Guid.Empty)
                throw new ArgumentException("FromUserId must be valid.", nameof(fromUserId));
            if (toUserId == Guid.Empty)
                throw new ArgumentException("ToUserId must be valid.", nameof(toUserId));
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

            TripId = tripId;
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Rating = rating;
            Comment = comment;
            CreatedDate = DateTime.UtcNow;
        }

        // Just for data seeding
        public Feedback()
        {

        }
    }
}