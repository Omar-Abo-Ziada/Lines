using Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.DTOs;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.CreateDriverEmergencyContact;

public class CreateDriverEmergencyContactResponse
{
    public CreateDriverEmergencyContactDto EmergencyContact { get; set; } = default!;
    public string Message { get; set; } = "Emergency contact created successfully";
}

