



namespace Lines.Presentation.Endpoints.TripRequests.EditTripRequestPrice
{
    // ?? Response Model
    public record EditTripRequestPriceResponse(
        Guid TripRequestId,
        decimal NewPrice,
        string Message = "Trip request price updated successfully."
    );
}


 