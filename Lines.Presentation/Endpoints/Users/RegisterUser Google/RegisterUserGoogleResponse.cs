using Lines.Application.Features.Users.Register_User.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Users.RegisterUser_Google;

public record RegisterUserGoogleResponse(string Token, string Role);

public class RegisterUserGoogleResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserDTO, RegisterUserGoogleResponse>();
    }
}