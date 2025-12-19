using Lines.Domain.Models.Common;
using Lines.Domain.Value_Objects;

namespace Lines.Domain.Models.Drivers;

public class DriverEmergencyContact : BaseModel
{
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    
    public string Name { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    // Parameterless constructor for EF Core
    public DriverEmergencyContact() { }

    public DriverEmergencyContact(Guid driverId, string name, PhoneNumber phoneNumber)
    {
        ValidateEmergencyContactDetails(driverId, name, phoneNumber);
        
        DriverId = driverId;
        Name = name;
        PhoneNumber = phoneNumber.Value.ToString();
    }

    private void ValidateEmergencyContactDetails(Guid driverId, string name, PhoneNumber phoneNumber)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("Driver ID cannot be empty", nameof(driverId));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
            
        if (phoneNumber == null || string.IsNullOrWhiteSpace(phoneNumber.Value.ToString()))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
    }

    // Business methods
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty", nameof(newName));
        Name = newName;
    }

    public void UpdatePhoneNumber(PhoneNumber newPhoneNumber)
    {
        if (newPhoneNumber == null || string.IsNullOrWhiteSpace(newPhoneNumber.Value.ToString()))
            throw new ArgumentException("Phone number cannot be empty", nameof(newPhoneNumber));
        PhoneNumber = newPhoneNumber.Value.ToString();
    }
}

