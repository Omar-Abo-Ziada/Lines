using Lines.Domain.Models.Common;
using Location = Lines.Domain.Value_Objects.Location;

namespace Lines.Domain.Models.Trips
{
    //[Table("EndTripLocations", Schema = "Trips")]
    public class EndTripLocation : BaseModel
    {
        public Guid? TripId { get;  set; }
        public virtual Trip? Trip { get;  set; }
        public Guid TripRequestId { get; set; }
        public virtual TripRequest TripRequest { get; set; }
        public Location  Location { get; set; }
        public int Order { get;  set; }

        public EndTripLocation(Guid tripRequestId, Location location , int order)
        {
            TripRequestId = tripRequestId;
            Location = location;    
            Order = order;
        }

        // Just for data seeding
        public EndTripLocation()
        {

        }
    }
}
