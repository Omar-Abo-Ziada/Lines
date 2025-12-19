namespace Lines.Presentation.Endpoints.Passengers.GetAllPassengers
{
    public record GetAllPassengersRequest (string? FirstName, string? LastName , string? Email , string? PhoneNumber,
        int PageNumber = 1, int PageSize = 10);
}
