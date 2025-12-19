using Lines.Domain.Models.Drivers;
using Mapster;

namespace Lines.Application.Features.Drivers.GetDriverContact.DTOs;

public class DriverContactDto
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class DriverContactDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Driver, DriverContactDto>()
            .Map(dest => dest.Email, src => src.Email.Value)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber.Value);
    }
}
