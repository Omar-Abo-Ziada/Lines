using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.LoginUser_Google
{
    public record LoginUserGoogleRequest();

    public class LoginUserGoogleRequestValidator : AbstractValidator<LoginUserGoogleRequest>
    {
        public LoginUserGoogleRequestValidator()
        {
            // No validation needed since the orchestrator handles all logic
        }
    }
}
