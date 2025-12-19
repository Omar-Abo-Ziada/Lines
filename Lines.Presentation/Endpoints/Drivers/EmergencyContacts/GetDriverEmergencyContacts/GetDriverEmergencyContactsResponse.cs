using Lines.Application.Features.DriverEmergencyContacts.Shared.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.GetDriverEmergencyContacts;

public record GetDriverEmergencyContactsResponse(
    List<DriverEmergencyContactDto> EmergencyContacts
);

public class GetDriverEmergencyContactsResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<List<DriverEmergencyContactDto>, GetDriverEmergencyContactsResponse>()
            .Map(dest => dest.EmergencyContacts, src => src);
    }
}

