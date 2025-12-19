using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Presentation.Endpoints.Users.LoginUser_Google;
using Mapster;

namespace Lines.Presentation.Endpoints.Users.LoginUserApple
{
    public record LoginUserAppleResponse(string Token, string Role);

    public class LoginUserAppleResponseMappingConfig : IRegister
    {
      
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LoginDTO, LoginUserGoogleResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest.Role, src => src.Role);
        }
    }


    
}
