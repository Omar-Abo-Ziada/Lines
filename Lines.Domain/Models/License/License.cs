using Lines.Domain.Enums;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.License
{
    public abstract class License : BaseModel
    {
        public string LicenseNumber { get;  set; }
        public DateTime IssueDate { get;  set; }
        public DateTime ExpiryDate { get;  set; }
        public bool IsValid { get;  set; }
        public LicenseType licenseType { get;  set; }
        public virtual ICollection<LicensePhoto> Photos { get; set; }

        protected License(string licenseNUmber , DateTime issueDate , DateTime expiryDate, ICollection<LicensePhoto> photos)
        {
            ValidateLicenseDetails(licenseNUmber , issueDate , expiryDate, photos);
            LicenseNumber = licenseNUmber;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            Photos = photos;
            Validate();
        }

        // Just for data seeding
        public License()
        {

        }
        public virtual bool IsExpired()
        {
            return ExpiryDate <= DateTime.UtcNow;
        }

        public virtual void Invalidate()
        {
            IsValid = false;
        }

        public virtual void Validate()
        {
            if (!IsExpired())
                IsValid = true;
        }

        public virtual void Delete()
        {
            if (!IsDeleted)
                IsDeleted = true;
        }

        protected void ValidateLicenseDetails(string licenseNumber, DateTime issueDate,
        DateTime expiryDate, ICollection<LicensePhoto> photos)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("License number cannot be empty");

            if (issueDate > DateTime.UtcNow)
                throw new ArgumentException("Issue date cannot be in the future");

            if (expiryDate <= DateTime.UtcNow)
                throw new ArgumentException("License has expired");

            if (expiryDate <= issueDate)
                throw new ArgumentException("Expiry date must be after issue date");

            if(photos == null || photos.Count == 0)
            {
                throw new ArgumentException("Any license must have 1 photo at least");
            }
        }

        public virtual void AddPhoto(LicensePhoto photo)
        {
            if (photo == null)
            {
                throw new ArgumentNullException(nameof(photo));
            }

            // Ensure the photo is associated with this license
            photo.LicenseId = this.Id;
            Photos.Add(photo);
        }
    }
}







