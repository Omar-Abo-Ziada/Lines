using Lines.Domain.Models.Trips;
using Lines.Domain.Models.Vehicles;
using Lines.Domain.Value_Objects;
using Lines.Domain.Enums;


namespace Lines.Domain.Models.Drivers;


public class Driver : Lines.Domain.Models.Users.User
{
    public Location? Location { get; set; }
    readonly TimeSpan LocationStaleThreshold = TimeSpan.FromMinutes(1);  // can remove it if using signalr to update live location each second
    // Status and Rating
    public bool IsAvailable { get; set; } = true; //when assigned on trip make it false , after completion return it true
    public bool IsNotifiedForOnlyTripsAboveMyPrice { get; set; }
    
    // License relationship - one-to-many for license history
    public virtual ICollection<DriverLicense> Licenses { get; set; } = new List<DriverLicense>();
    public virtual ICollection<Offer> Offers { get; set; }
    public virtual ICollection<DriverServiceFeeOffer> ServiceFeeOffers { get; set; } = new List<DriverServiceFeeOffer>();
    public virtual ICollection<Trip>? Trips { get; set; }
    
    // Wallet and Offer Activations
    //public virtual Wallet Wallet { get; set; }
    public virtual ICollection<DriverOfferActivation> OfferActivations { get; set; } = new List<DriverOfferActivation>();

    // Vehicle relationship changed from one-to-one to one-to-many
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public virtual ICollection<TripRequest>? TripRequests { get; set; }
    public virtual ICollection<Earning>? Earnings { get; set; }

    // Multi-step registration properties
    public string? CompanyName { get; set; }
    public string? CommercialRegistration { get; set; } // CHE
    public DateTime? DateOfBirth { get; set; }
    public IdentityType? IdentityType { get; set; }
    public string? SecondaryPhoneNumber { get; set; }
    public string? PersonalPictureUrl { get; set; }
    
    // Address navigation
    public virtual ICollection<DriverAddress> Addresses { get; set; } = new List<DriverAddress>();
    
    // Bank account navigation - one-to-many relationship
    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    
    // Registration progress flags
    public bool PersonalInfoCompleted { get; set; }
    public bool ContactInfoCompleted { get; set; }
    public bool AddressCompleted { get; set; }
    public bool LicenseCompleted { get; set; }
    public bool VehicleCompleted { get; set; }
    public bool WithdrawalCompleted { get; set; }
    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.PendingReview;


    // Constructor
    public Driver() : base(Guid.Empty, string.Empty, string.Empty, new Email(string.Empty), new PhoneNumber(string.Empty)) { }

    public Driver(Guid id, string firstName, string lastName, Email email, PhoneNumber phoneNumber,
                  bool isNotifiedForOnlyTripsAboveMyPrice = false)
  : base(id, firstName, lastName, email, phoneNumber)
    {
        ValidateDriverDetails(id, firstName, lastName);
        this.IsNotifiedForOnlyTripsAboveMyPrice = isNotifiedForOnlyTripsAboveMyPrice;
    }

    // Business Rules Methods
    private void ValidateDriverDetails(Guid id, string firstName, string lastName)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty");

        //if (driverLicenseId == Guid.Empty)
        //    throw new ArgumentException("Driver license ID cannot be empty");
    }


    // Location tracking methods
    public void UpdateLocation(double latitude, double longitude, Guid cityId, Guid sectorId)
    {
        ValidateLocationUpdate(latitude, longitude);
        Location = new Location() { Latitude = latitude, Longitude = longitude };
    }

    void ValidateLocationUpdate(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude. Must be between -90 and 90 degrees.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude. Must be between -180 and 180 degrees.");
    }

    // Public Methods
    public void SetAvailability(bool available)
    {
        IsAvailable = available;

    }


    // Multi-step registration methods
    public void UpdatePersonalInfo(string? companyName, string? commercialRegistration, DateTime? dateOfBirth, 
        IdentityType? identityType, string? personalPictureUrl)
    {
        CompanyName = companyName;
        CommercialRegistration = commercialRegistration;
        DateOfBirth = dateOfBirth;
        IdentityType = identityType;
        PersonalPictureUrl = personalPictureUrl;
        PersonalInfoCompleted = true;
    }

    public void UpdateContactInfo(string? secondaryPhoneNumber)
    {
        SecondaryPhoneNumber = secondaryPhoneNumber;
        ContactInfoCompleted = true;
    }

    public void MarkLicenseCompleted()
    {
        LicenseCompleted = true;
    }

    public void MarkVehicleCompleted()
    {
        VehicleCompleted = true;
    }

    public void MarkWithdrawalCompleted()
    {
        WithdrawalCompleted = true;
    }

    public bool IsRegistrationComplete()
    {
        return PersonalInfoCompleted && ContactInfoCompleted && AddressCompleted && 
               LicenseCompleted && VehicleCompleted && WithdrawalCompleted;
    }

    public void AddBankAccount(BankAccount bankAccount)
    {
        if (bankAccount == null)
            throw new ArgumentNullException(nameof(bankAccount), "Bank account cannot be null");

        if (bankAccount.DriverId != Id)
            throw new ArgumentException("Bank account must belong to this driver", nameof(bankAccount));

        BankAccounts.Add(bankAccount);
    }

    public void AddLicense(DriverLicense license)
    {
        if (license == null)
            throw new ArgumentNullException(nameof(license), "License cannot be null");

        if (license.DriverId != Id)
            throw new ArgumentException("License must belong to this driver", nameof(license));

        // If this license is marked as current, ensure no other license is marked as current
        if (license.IsCurrent)
        {
            // Set all existing licenses as history first
            foreach (var existingLicense in Licenses.ToList())
            {
                existingLicense.SetAsHistory();
            }
        }
        
        Licenses.Add(license);
    }

    public DriverLicense? GetCurrentLicense()
    {
        return Licenses.FirstOrDefault(l => l.IsCurrent && l.IsActive);
    }
}