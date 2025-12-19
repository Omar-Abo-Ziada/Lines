using FluentValidation;

namespace Lines.Presentation.Endpoints.Users.RegisterUser_Google;

public record RegisterUserGoogleRequests;

public class RegisterUserGoogleRequestsValidator : AbstractValidator<RegisterUserGoogleRequests>
{
    public RegisterUserGoogleRequestsValidator()
    {
        // No validation needed since the orchestrator handles all logic
    }
}