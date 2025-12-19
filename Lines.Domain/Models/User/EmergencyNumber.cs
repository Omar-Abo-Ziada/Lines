using Lines.Domain.Enums;
using Lines.Domain.Models.Common;
using Lines.Domain.Value_Objects;

namespace Lines.Domain.Models.Users
{
    public class EmergencyNumber : BaseModel
    {
        public string Name { get;  set; } // e.g., "Police", "Ambulance", "Fire Department"
        public string PhoneNumber { get;  set; }  // make it string not phone number type to be saved in sql properly
        public EmergencyNumberType EmergencyNumberType { get;  set; }
        public Guid? UserId { get;  set; }

        // Constructor
        public EmergencyNumber(string name, PhoneNumber phoneNumber, EmergencyNumberType emergencyNumberType, Guid? userId) // may no user as it is a shared emergency number
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            if (UserId == Guid.Empty)
                throw new ArgumentException("User id cannot be empty.", nameof(userId));

            Name = name;
            PhoneNumber = phoneNumber.Value.ToString();
            EmergencyNumberType = emergencyNumberType;
            UserId = userId;
        }

        // Just for data seeding
        public EmergencyNumber()
        {

        }

        // Business Methods

        public void UpdatePhoneNumber(PhoneNumber newPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(newPhoneNumber))
                throw new ArgumentException("Phone number cannot be empty.", nameof(newPhoneNumber));
            PhoneNumber = newPhoneNumber.Value.ToString();
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty.", nameof(newName));
            Name = newName;
        }
    }
}