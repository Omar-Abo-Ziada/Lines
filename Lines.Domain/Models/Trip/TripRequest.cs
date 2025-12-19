using Lines.Domain.Enums;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Passengers;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Models.User;
using Lines.Domain.Models.Vehicles;
using Lines.Domain.Value_Objects;

namespace Lines.Domain.Models.Trips;

public class TripRequest : BaseModel
{
    public DateTime RequestedAt { get; set; }
    public TripRequestStatus Status { get; set; }
    public string? CancellationReason { get; set; }
    public bool IsScheduled { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public decimal EstimatedPrice { get; set; }
    public double Distance { get; set; }
    public virtual ICollection<Offer>? Offers { get; set; }
    // Optional: Assigned Driver (when a driver accepts the request)
    public Guid? DriverId { get; set; }
    public virtual Driver? Driver { get; set; }
    // Requester
    public Guid PassengerId { get; set; }
    public virtual Passenger Passenger { get; set; }

    // Request Details
    public virtual Location StartLocation { get; set; }
    public virtual ICollection<EndTripLocation> EndLocations { get; set; }

    public Guid VehicleTypeId { get; set; }
    public virtual VehicleType VehicleType { get; set; }

    public Guid PaymentMethodId { get; set; }
    public virtual PaymentMethod PaymentMethod { get; set; }

    public Guid? TripId { get; set; }
    public virtual Trip Trip { get; set; }
    public Guid? UserRewardId { get; set; }
    public virtual UserReward? UserReward { get; set; }

    private const int MaxEndLocations = 5;               // will not be mapped to db as it is a private const field
    // Constructor for creating a new trip request
    public TripRequest(Guid passengerId, Location startLocation, Guid vehicleType, Guid paymentMethodId, bool isScheduled, DateTime? scheduledAt, decimal? estimatedPrice, double distance,
         ICollection<EndTripLocation> endTripLocations, Guid? userRewardId = null)
    {
        Validate(passengerId, startLocation, vehicleType, paymentMethodId, endTripLocations);

        // Potentially add validation for pickup and drop-off locations being different, etc.

        PassengerId = passengerId;
        StartLocation = startLocation;
        RequestedAt = DateTime.UtcNow;
        Status = TripRequestStatus.Pending; // Initial status
        VehicleTypeId = vehicleType;
        IsScheduled = isScheduled;
        ScheduledAt = scheduledAt;
        PaymentMethodId = paymentMethodId;
        EstimatedPrice = estimatedPrice ?? 0;
        Distance = distance;
        EndLocations = endTripLocations;
        UserRewardId = userRewardId;
    }

    // Just for data seeding
    public TripRequest()
    {

    }


    private static void Validate(Guid passengerId, Location startLocation, Guid vehicleType, Guid paymentMethodId, ICollection<EndTripLocation> endTripLocations)
    {
            if (passengerId == Guid.Empty)
                throw new ArgumentException("PassengerId must be valid.", nameof(passengerId));
            if (paymentMethodId == Guid.Empty)
                throw new ArgumentNullException(nameof(paymentMethodId), "PaymentMethodId is required.");

        if (startLocation == null)
            throw new ArgumentNullException(nameof(startLocation));
        if (vehicleType == Guid.Empty)
            throw new ArgumentNullException(nameof(vehicleType));

        if (endTripLocations == null || endTripLocations.Count == 0)
            throw new ArgumentException("At least one EndLocation is required.", nameof(endTripLocations));
        if (endTripLocations.Count > MaxEndLocations)
            throw new ArgumentException($"You can add at most {MaxEndLocations} EndLocations.", nameof(endTripLocations));
    }

    // Business Methods

    public void AcceptRequest(Guid driverId, DateTime estimatedPickupTime)
    {
        if (Status != TripRequestStatus.Pending)
            throw new InvalidOperationException("Request can only be accepted if it is pending.");
        if (driverId == Guid.Empty)
            throw new ArgumentException("DriverId must be valid.");
        if (estimatedPickupTime < DateTime.UtcNow)
            throw new ArgumentException("Estimated pickup time cannot be in the past.");

        DriverId = driverId;
        Status = TripRequestStatus.Accepted;
        //TODO: raise a domain event: TripRequestAccepted
    }

    public void CancelRequestByPassenger(string cancellationReason)
    {
        // Allow cancellation if pending or accepted (before driver arrives, for example)
        // Business rule: Can a passenger cancel an accepted request? If so, under what conditions?
        if (Status == TripRequestStatus.CancelledByPassenger)
            throw new InvalidOperationException($"Cannot cancel request with status {Status}.");

        Status = TripRequestStatus.CancelledByPassenger;
        CancellationReason = cancellationReason;
        UserRewardId = null;
    }

    public void CancelRequestByDriver()
    {
        // Business rule: Can a driver cancel an accepted request? If so, under what conditions?
        if (Status != TripRequestStatus.Accepted)
            throw new InvalidOperationException("Driver can only cancel an accepted request.");

        Status = TripRequestStatus.Pending;  // to allow other drivers to apply
        DriverId = null; // Unassign driver
                         //TODO: TripRequestCancelledByDriver
                         // May need to revert status to Pending or a new status like "Reopened" for other drivers to pick up.
    }

    public void AddOffer(Offer offer)
    {
        if (offer != null)
        {
            Offers?.Add(offer);
        }
    }


    public bool IsValidPrice(decimal vehicleTypePerKmCharge)
    {
        if (CalculateMinPrice(vehicleTypePerKmCharge) > EstimatedPrice)
        {
            return false;
        }
        return true;
    }


    public decimal CalculateMinPrice(decimal vehicleTypePerKmCharge)
    {
        var vehicleTypeCharge = vehicleTypePerKmCharge * (decimal)Distance;

        return vehicleTypeCharge - (vehicleTypeCharge * 0.2m);
    }

    public void AssignTrip(Guid tripId)
    {
        TripId = tripId;
    }
}