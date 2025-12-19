namespace Lines.Presentation.Endpoints.Trips.PassengerRequestStart
{
    public record PassengerRequestStartRequest(Guid TripId);

    public class PassengerRequestStartValidator : AbstractValidator<PassengerRequestStartRequest>
    {
        public PassengerRequestStartValidator()
        {
            RuleFor(x => x.TripId)
                .NotEmpty()
                .WithMessage("TripId is required.");
        }
    }
}
