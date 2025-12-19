using Lines.Domain.Enums;
using Lines.Domain.Models.Chats;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Passengers;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Value_Objects;

namespace Lines.Domain.Models.Trips;

//[Table("Trips", Schema = "Trips")]
public class Trip : BaseModel
{
    // Trip details
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TripStatus Status { get; set; }
    // ، ده الوقت المتوقع لوصول السائق للمكان اللي الراكب محدده ، وبيجي من الفرونت، ولما السائق يصل بنحدثه بالوقت اللي السائق وصل فيه،  فيصبح هو وقت وصول السائق فعلاً
    public DateTime? EstimatedPickupTime { get; set; } // Could be set when a driver accepts
    public bool IsPaid { get; set; }
    public bool IsRewardApplied { get; set; }
    public decimal? FareAfterRewardApplied { get; set; }

    // Participants
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    public Guid PassengerId { get; set; }
    public virtual Passenger Passenger { get; set; }

    public Location? StartLocation { get; set; }
    public virtual ICollection<EndTripLocation> EndLocations { get; set; }
    public double? Distance { get; set; }  //DistanceCalculator.CalculateTotalDistance(StartLocation, EndLocations);
                                           //public Guid? FeedbackId { get; set; }
    public virtual ICollection<Feedback>? Feedbacks { get; set; }
    public decimal Fare { get; set; }
    public decimal Tips { get; set; } = 0;
    public string Currency { get; set; } = "QAR";
    // Rating logic handled by Feedback entity - see User.Rating for driver average
    public Guid PaymentMethodId { get; set; }
    public virtual PaymentMethod? PaymentMethod { get; set; }
    public PaymentMethodType? PaymentMethodType { get; set; }
    public Guid? PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    public Guid TripRequestId { get; set; }

    public virtual TripRequest TripRequest { get; set; }
    public virtual ICollection<ChatMessage>? Messages { get; set; }

   
    public string? TripCode { get; private set; }


    public Trip(Guid driverId, Guid passengerId, Location startLocation, double distance,
                decimal fare, Guid paymentMethodId, Guid tripRequestId, PaymentMethodType paymentMethodType,
                string currency = "QAR", decimal tips = 0, bool isPaid = false, bool isRewardApplied = false, decimal? fareAfterRewardApplied = null)
    {
        if (tripRequestId == Guid.Empty) throw new ArgumentException("Trip request id must be valid.");
        if (driverId == Guid.Empty) throw new ArgumentException("DriverId must be valid.");
        if (distance <= 0) throw new ArgumentException("Distance must be greater than zero.");
        if (fare <= 0) throw new ArgumentException("Fare must be greater than zero.");
        if (startLocation == null) throw new ArgumentNullException(nameof(startLocation));
        if (tips < 0) throw new ArgumentException("Tips cannot be negative.");

        DriverId = driverId;
        PassengerId = passengerId;
        StartLocation = startLocation;
        Fare = fare;
        Tips = tips;
        Currency = currency;
        PaymentMethodId = paymentMethodId;
        PaymentMethodType = paymentMethodType;
        TripRequestId = tripRequestId;
        IsPaid = isPaid;
        IsRewardApplied = isRewardApplied;
        FareAfterRewardApplied = fareAfterRewardApplied;
    }


    // Just for data seeding
    public Trip()
    {

    }
    // Business Methods


   



    public void StartTrip()
    {
        StartedAt = DateTime.UtcNow;
        Status = TripStatus.InProgress;
    }

    public void CompleteTrip()
    {
        if (Status != TripStatus.InProgress)
            throw new InvalidOperationException("Trip can only be completed if it is in progress.");

        EndedAt = DateTime.UtcNow;
        Status = TripStatus.Completed;
    }

    public void CancelTrip()
    {
        if (Status == TripStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed trip.");
        Status = TripStatus.Cancelled;
    }

    public void SetPayment(Payment payment)
    {
        if (Status == TripStatus.Cancelled)
            throw new ArgumentException("Payment occurs for completed or in progress trips only");

        if (payment != null)
        {
            Payment = payment;
        }
    }

    public void SetFareAfterRewardApplied(decimal discountPercentage, decimal maxValue)
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
        FareAfterRewardApplied = Math.Max(0, Fare - appliedDiscount);

        IsRewardApplied = true;
    }

    public void SetTripCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Trip code cannot be empty.");

        TripCode = code;
    }

}