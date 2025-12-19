using System.ComponentModel.DataAnnotations.Schema;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.Drivers
{
    //[Table("Earnings", Schema = "Driver")]
    public class Earning : BaseModel // represent the money owed by the system to the driver
    {
        public Guid DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public decimal Amount { get; set; }
        public DateTime EarnedAt { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid? TripId { get; set; }
        public virtual Trip? Trip { get; set; }


        // Constructor
        public Earning(Guid driverId, Guid tripId, decimal amount, Guid paymentId)
        {
            if (driverId == Guid.Empty)
                throw new ArgumentException("DriverId must be valid.", nameof(driverId));
            if (tripId == Guid.Empty)
                throw new ArgumentException("TripId must be valid.", nameof(tripId));
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            DriverId = driverId;
            TripId = tripId;
            PaymentId = paymentId;
            Amount = amount;
            EarnedAt = DateTime.UtcNow;
            IsPaid = false;                  // by default money go to the system so earning is not paid to the driver yet
        }


        // Just for data seeding
        public Earning()
        {

        }
        // Business Methods

        public void MarkAsPaid()
        {
            if (IsPaid)
                throw new InvalidOperationException("Earning is already marked as paid.");
            IsPaid = true;
            PaidAt = DateTime.UtcNow;
        }

        public void UpdateAmount(decimal newAmount)
        {
            if (newAmount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(newAmount));
            Amount = newAmount;
        }
    }
}