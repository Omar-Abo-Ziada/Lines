using System.ComponentModel.DataAnnotations.Schema;
using Lines.Domain.Enums;
using Lines.Domain.Models.License;


namespace Lines.Domain.Models.Drivers
{
    //[Table("DriverLicenses", Schema = "Driver")]
    public class DriverLicense : License.License
    {
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }
        
        public bool IsCurrent { get; set; } // Is this the active license?
        public bool IsActive { get; set; } = true; // For soft deletion

        public DriverLicense(string licenseNumber, DateTime issueDate, DateTime expiryDate,
           Guid driverId, ICollection<LicensePhoto> photos, bool isCurrent = true) : base(licenseNumber, issueDate, expiryDate, photos)
        {
            if (driverId == Guid.Empty)
                throw new ArgumentException("driverId must be valid.", nameof(driverId));

            DriverId = driverId;
            licenseType = LicenseType.DriverLicense;
            IsCurrent = isCurrent;
            IsActive = true;
        }


        // just for data seeding
        public DriverLicense()
        {

        }

        public bool CanDrive()
        {
            return IsValid && !IsExpired();
        }

        public void SetAsCurrent()
        {
            IsCurrent = true;
        }

        public void SetAsHistory()
        {
            IsCurrent = false;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}