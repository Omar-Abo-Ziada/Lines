using Lines.Application.Features.Users.LoginUser.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Users.LoginUser_Google;

public record LoginUserGoogleResponse(string Token, string Role);

public class LoginUserGoogleResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LoginDTO, LoginUserGoogleResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest.Role, src => src.Role);
    }
}