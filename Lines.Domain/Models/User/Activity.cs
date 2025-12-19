using Lines.Domain.Models.Common; 

namespace Lines.Domain.Models.Users
{
    public class Activity : BaseModel
    {
        public Guid UserId { get; set; }
        public DateOnly Day { get; set; }


        // Constructor for starting an activity
        public Activity(Guid appUserId, DateOnly day)
        {
            if (appUserId == Guid.Empty)
                throw new ArgumentException("AppUserId must be valid.", nameof(appUserId));
            if (day > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentOutOfRangeException(nameof(day), "Day cannot be in the future.");

            UserId = appUserId;
            Day = day;
        }


        // Just for data seeding
        public Activity()
        {

        }

        // Business Methods
      

    }
}