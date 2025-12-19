using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Vehicles
{
    public class VehiclePhoto : BaseModel
    {
        public Guid VehicleId { get;  set; }
        public virtual Vehicle Vehicle { get;  set; }
        public string PhotoUrl { get;  set; }
        public string? Description { get;  set; } // Optional
        public bool IsPrimary { get;  set; } // Is this the main photo for the vehicle?

        public VehiclePhoto(Vehicle vehicle, string photoUrl, bool isPrimary = false, string description = null)
        {
            if (vehicle == null) throw new ArgumentException("vehicle is required.", nameof(vehicle));
            if (string.IsNullOrWhiteSpace(photoUrl)) throw new ArgumentException("PhotoUrl is required.", nameof(photoUrl));

            VehicleId = vehicle.Id;
            PhotoUrl = photoUrl;
            IsPrimary = isPrimary;
            Description = description;
            Vehicle = vehicle;
        }


        // Just for data seeding
        public VehiclePhoto()
        {

        }

        public void SetAsPrimary() => IsPrimary = true;
        public void UnsetAsPrimary() => IsPrimary = false;
    }
}