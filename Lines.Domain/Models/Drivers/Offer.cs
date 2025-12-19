using Lines.Domain.Models.Common;
using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.Drivers;

//[Table("Offers", Schema = "Driver")]
public class Offer : BaseModel
{
    public int TimeToArriveInMinutes { get; set; }
    public float DistanceToArriveInMeters { get; set; }
    public decimal Price { get; set; }

    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }

    // Status
    public bool IsAccepted { get; set; }
    public Guid TripRequestId { get; set; }
    public virtual TripRequest TripRequest { get; set; }

    public Offer(
        Guid driverId,
        int timeToArriveInMinutes,
        float distanceToArriveInMeters,
         decimal price,
         Guid tripRequestId)
    {
        ValidateOfferDetails(driverId, timeToArriveInMinutes, distanceToArriveInMeters, price, tripRequestId);

        DriverId = driverId;
        TimeToArriveInMinutes = timeToArriveInMinutes;
        DistanceToArriveInMeters = distanceToArriveInMeters;
        TripRequestId = tripRequestId;
        Price = price;
    }

    // Just for data seeding
    public Offer()
    {

    }
    void ValidateOfferDetails(
        Guid driverId,
        int TimeToArriveInMinutes,
        float DistanceToArriveInMeters,
        decimal price,
        Guid tripRequestId)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("Invalid driver id");

        if (TimeToArriveInMinutes < 0)
            throw new ArgumentException("Invalid remaining time");

        if (DistanceToArriveInMeters < 0)
            throw new ArgumentException("Invalid remaining distance");

        if (price <= 0)
            throw new ArgumentException("Invalid price");

        if (tripRequestId == Guid.Empty)
            throw new ArgumentException("Invalid trip request id");
    }
    public void MarkAsAccepted()
    {
        IsAccepted = true;
    }
}