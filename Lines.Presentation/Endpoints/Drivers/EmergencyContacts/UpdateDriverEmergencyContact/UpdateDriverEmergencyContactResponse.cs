using Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.DTOs;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.UpdateDriverEmergencyContact;

public class UpdateDriverEmergencyContactResponse
{
    public UpdateDriverEmergencyContactDto EmergencyContact { get; set; } = default!;
    public string Message { get; set; } = "Emergency contact updated successfully";
}

