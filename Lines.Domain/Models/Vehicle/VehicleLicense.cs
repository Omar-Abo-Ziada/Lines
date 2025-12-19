using Lines.Domain.Enums;
using Lines.Domain.Models.License;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lines.Domain.Models.Vehicles
{
    //[Table("VehicleLicenses", Schema = "Vehicle")]
    public class VehicleLicense : License.License
    {
        public Guid VehicleId { get;  set; }
        public Vehicle Vehicle { get;  set; }

        // Constructor
        public VehicleLicense(string licenseNumber, DateTime issuedAt, DateTime expiryAt, Guid vehicleId,
            ICollection<LicensePhoto> photos)
            : base(licenseNumber , issuedAt , expiryAt, photos)
        {
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("vehicleId must be valid.", nameof(vehicleId));

            ValidateLicenseDetails(licenseNumber, issuedAt, expiryAt, photos);
            VehicleId = vehicleId;
            licenseType = LicenseType.VehicleLicense;
        }

        // for data seeding
        public VehicleLicense()
        {
            
        }
        // Business Methods

    }
}