namespace Lines.Presentation.Endpoints.Passengers.UpdatePassengerProfile
{
    public record UpdatePassengerProfileRequest( string? FirstName, string? LastName , string? PhoneNumber);

    //public class UpdatePassengerProfileRequestValidator : AbstractValidator<UpdatePassengerProfileRequest>
    //{
    //    public UpdatePassengerProfileRequestValidator()
    //    {
         

    //        RuleFor(x => x.PhoneNumber)
    //        .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format")
    //        .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

    //    }
    //}
}
