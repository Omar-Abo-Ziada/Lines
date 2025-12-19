using Lines.Application.Features.Users.LoginUser.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Users.LoginPassenger
{
    public record LoginUserResponse (string Token , string Role);


    public class LoginUserResponseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LoginDTO, LoginUserResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest.Role, src => src.Role);
        }

    }

}
