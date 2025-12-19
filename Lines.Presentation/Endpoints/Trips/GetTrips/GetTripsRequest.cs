namespace Lines.Presentation.Endpoints.Trips.GetTrips;

public record GetTripsRequest(int? Status, int PageIndex = 1, int PageSize = 10);