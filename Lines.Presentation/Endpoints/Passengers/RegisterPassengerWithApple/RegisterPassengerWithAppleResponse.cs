using Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs;
using Lines.Application.Features.Users.Register_User.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Passengers.RegisterPassengerWithApple
{

    public record RegisterPassengerWithAppleResponse(string Token, string Role);

    public class RegisterPassengerWithAppleResponseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterPassengerWithAppleDTO, RegisterPassengerWithAppleResponse>();
        }
    }
}
