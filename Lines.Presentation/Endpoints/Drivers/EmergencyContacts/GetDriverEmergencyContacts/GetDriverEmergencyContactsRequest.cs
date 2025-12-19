using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.GetDriverEmergencyContacts;

public class GetDriverEmergencyContactsRequest
{
    public Guid? DriverId { get; set; }
}

public class GetDriverEmergencyContactsRequestValidator : AbstractValidator<GetDriverEmergencyContactsRequest>
{
    public GetDriverEmergencyContactsRequestValidator()
    {
        // DriverId is optional - if not provided, will use authenticated driver's ID
    }
}

