using System.ComponentModel.DataAnnotations.Schema;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Sites;
using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.Vehicles
{
    //[Table("VehicleTypes", Schema = "Vehicle")]
    public class VehicleType : BaseModel
    {
        public string Name { get;  set; } // e.g., Sedan, SUV, Van, Bike
        public string? Description { get;  set; }
        public int Capacity { get;  set; } // Max passengers
        public decimal PerKmCharge { get;  set; }
        public decimal PerMinuteDelayCharge { get;  set; }  // each delay min from the passenger after the first 5 min add cost to the trip 

        public decimal AverageSpeedKmPerHour { get; set; }  //Average speed in kilometers per hour

        public virtual ICollection<Vehicle> Vehicles { get;  set; }
        public virtual ICollection<TripRequest> TripRequests { get;  set; }
        public ICollection<CityVehicleType> CityVehicleTypes { get; set; }

        public VehicleType(string name, string description, int capacity, decimal perKmCharge, decimal perMinuteDelayCharge , decimal avgSpeedKmPerHour)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive.");
            if (perKmCharge < 0) throw new ArgumentOutOfRangeException(nameof(perKmCharge), "Per km charge cannot be negative.");
            if (perMinuteDelayCharge < 0) throw new ArgumentOutOfRangeException(nameof(perMinuteDelayCharge), "Per minute delay charge cannot be negative.");
            if (avgSpeedKmPerHour <= 0) throw new ArgumentOutOfRangeException(nameof(avgSpeedKmPerHour));

            Name = name;
            Description = description;
            Capacity = capacity;
            PerKmCharge = perKmCharge;
            PerMinuteDelayCharge = perMinuteDelayCharge;
            Vehicles = new List<Vehicle>();
            AverageSpeedKmPerHour = avgSpeedKmPerHour;
        }

        // Just for data seeding
        public VehicleType()
        {

        }

        public void UpdatePricing(decimal newPerKmCharge , decimal newPerMinuteDelayCharge)
        {
            if (newPerKmCharge < 0) throw new ArgumentOutOfRangeException(nameof(newPerKmCharge));
            if (newPerMinuteDelayCharge < 0) throw new ArgumentOutOfRangeException(nameof(newPerMinuteDelayCharge));
            PerKmCharge = newPerKmCharge;
            PerMinuteDelayCharge = newPerMinuteDelayCharge;
        }
    }
}