using Lines.Domain.Enums;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Drivers;

public class DriverRegistration : BaseModel
{
    public string RegistrationToken { get; set; } = string.Empty; // GUID for tracking
    public string? PhoneNumber { get; set; } // Initial identifier from Step 1
    public RegistrationStatus Status { get; set; }
    
    // Step 1: Personal Info
    public string? PersonalPictureUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? CommercialRegistration { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PasswordHash { get; set; }
    public IdentityType? IdentityType { get; set; }
    
    // Step 2: Contact Info
    public string? Email { get; set; } // Primary identifier for account creation
    
    // Email Verification
    public bool IsEmailVerified { get; set; } = false;
    public string? EmailVerificationCode { get; set; }
    public DateTime? EmailVerificationExpiry { get; set; }
    
    // Step 3: Address
    public Guid? CityId { get; set; }
    public Guid? SectorId { get; set; } // Province/Sector
    public string? Address { get; set; } // Renamed from Street
    public string? PostalCode { get; set; }
    public string? LimousineBadgeUrl { get; set; } // New field from screenshot
    
    // Step 4: License (JSON or separate table)
    public string? LicenseData { get; set; } // JSON: { number, expiry, imageUrls[] }
    
    // Step 5: Vehicle (JSON or separate table)
    public string? VehicleData { get; set; } // JSON: { typeId, model, year, color, plate, docs[], kmPrice }
    
    // Step 6: Withdrawal
    public string? BankAccountData { get; set; } // JSON: { bankName, iban, swift, holderName }
    
    // Progress flags
    public bool IsPersonalInfoCompleted { get; set; }
    public bool IsContactInfoCompleted { get; set; }
    public bool IsAddressCompleted { get; set; }
    public bool IsLicenseCompleted { get; set; }
    public bool IsVehicleCompleted { get; set; }
    public bool IsWithdrawalInfoCompleted { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    public Guid? CreatedDriverId { get; set; } // Set after finalization
}
