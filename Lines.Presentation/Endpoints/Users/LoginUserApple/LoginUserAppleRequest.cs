using Lines.Presentation.Endpoints.Users.LoginUser_Google;

namespace Lines.Presentation.Endpoints.Users.LoginUserApple
{
    public record LoginUserAppleRequest();

    public class LoginUserAppleRequestValidator : AbstractValidator<LoginUserAppleRequest>
    {
        public LoginUserAppleRequestValidator() 
        {
            // No validation needed since the orchestrator handles all logic
        }
    }


}
