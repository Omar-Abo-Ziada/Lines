using Lines.Application.Features.DriverBlockedPassengers.DTOs;

namespace Lines.Application.Features.DriverBlockedPassengers.GetBlockedPassengers.DTOs;

public class GetBlockedPassengersResponse
{
    public List<BlockedPassengerDto> BlockedPassengers { get; set; } = new();
    public int TotalCount { get; set; }
}

