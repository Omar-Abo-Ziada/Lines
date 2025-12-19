using Lines.Domain.Models.Common;
using Lines.Domain.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Models.License
{
    public class LicensePhoto : BaseModel 
    {
        public string PhotoUrl { get; set; }

        // Foreign key for the License
        public Guid LicenseId { get; set; }
        // Navigation property to the License
        public virtual License License { get; set; }

        public LicensePhoto(License license, string photoUrl)
        {
            if (license == null) throw new ArgumentException("license is required.", nameof(license));
            if (string.IsNullOrWhiteSpace(photoUrl)) throw new ArgumentException("PhotoUrl is required.", nameof(photoUrl));

            LicenseId = license.Id;
            PhotoUrl = photoUrl;
            License = license;
        }


        // Just for data seeding
        public LicensePhoto()
        {

        }

    }
}
