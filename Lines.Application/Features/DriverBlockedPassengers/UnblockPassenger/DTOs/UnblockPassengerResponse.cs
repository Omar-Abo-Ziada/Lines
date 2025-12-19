namespace Lines.Application.Features.DriverBlockedPassengers.UnblockPassenger.DTOs;

public class UnblockPassengerResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = "Passenger unblocked successfully";
}

