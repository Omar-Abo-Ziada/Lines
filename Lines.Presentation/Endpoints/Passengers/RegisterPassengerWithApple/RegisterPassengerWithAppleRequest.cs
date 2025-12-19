namespace Lines.Presentation.Endpoints.Passengers.RegisterPassengerWithApple
{

    public record RegisterPassengerWithAppleRequest;

    public class RegisterPassengerWithAppleRequestValidator : AbstractValidator<RegisterPassengerWithAppleRequest>
    {
        public RegisterPassengerWithAppleRequestValidator()
        {
            // No validation needed since the orchestrator handles all logic
        }
    }
}
