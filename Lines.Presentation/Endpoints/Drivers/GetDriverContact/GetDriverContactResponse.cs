using Lines.Application.Features.Drivers.GetDriverContact.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverContact;

public record GetDriverContactResponse(
    string Email,
    string PhoneNumber
);

public class GetDriverContactResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverContactDto, GetDriverContactResponse>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);
    }
}
