namespace Lines.Application.Features.DriverBlockedPassengers.BlockPassenger.DTOs;

public class BlockPassengerResponse
{
    public Guid BlockId { get; set; }
    public string Message { get; set; } = "Passenger blocked successfully";
}

